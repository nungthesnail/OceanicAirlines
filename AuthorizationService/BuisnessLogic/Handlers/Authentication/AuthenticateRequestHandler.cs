using BuisnessLogic.Handlers.Exceptions;
using BuisnessLogic.Models.Authentication;
using BuisnessLogic.Repository;
using InterserviceCommunication.Exceptions;
using InterserviceCommunication;
using JwtUtils;
using PasswordUtils;
using InterserviceCommunication.Models.UserService;


namespace BuisnessLogic.Handlers.Authentication
{
    /// <summary>
    /// Класс обработчика запроса прохождения процедуры аутентификации и авторизации
    /// </summary>
    public class AuthenticateRequestHandler
    {
        private RepositoryFacade _repositoryFacade;

        private InterserviceCommunicator _interserviceCommunicator;

        private JwtProvider _jwtProvider;

        private PasswordHasher _passwordHasher;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="repositoryFacade">Объект репозитория</param>
        /// <param name="interserviceCommunicator">Объект межсервисного связиста</param>
        /// <param name="jwtProvider">Провайдер JSON Web Token</param>
        /// <param name="passwordHasher">Объект хешера паролей</param>
        public AuthenticateRequestHandler(
            RepositoryFacade repositoryFacade,
            InterserviceCommunicator interserviceCommunicator,
            JwtProvider jwtProvider,
            PasswordHasher passwordHasher)
        {
            _repositoryFacade = repositoryFacade;
            _interserviceCommunicator = interserviceCommunicator;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Метод обработки запроса прохождения процедуры аутентификации и авторизации
        /// </summary>
        /// <param name="request">Запрос авторизации</param>
        /// <returns>Результат авторизации в формате JSON Web Token</returns>
        public async Task<AuthenticationResponse> Handle(AuthenticationRequest request)
        {
            await ThrowIfUserDoesntExists(request.Id);
            ThrowIfUserDoesntHavePassword(request.Id);

            var userId = request.Id;
            var userPassword = request.Password;
            var passwordHash = GetUserPasswordHash(userId);

            var verified = VerifyPassword(userPassword, passwordHash);

            ThrowIfVerificationFailed(verified);

            var token = await GenerateJwt(userId);

            var response = CreateResponse(token);

            return response;
        }

        private async Task ThrowIfUserDoesntExists(Guid linkedUserId)
        {
            var userExists = await UserExists(linkedUserId);

            if (!userExists)
            {
                throw new UserDoesntExistsException();
            }
        }

        private async Task<bool> UserExists(Guid linkedUserId)
        {
            try
            {
                var connector = _interserviceCommunicator.GetUserServiceConnector();
                var request = connector.CreateCheckUserExistsRequest(linkedUserId);
                var result = await request.Send();

                return result;
            }
            catch (InterserviceCommunicationException exc)
            {
                throw new InterserviceCommunicationFailedException(exc.Message);
            }
        }

        private void ThrowIfUserDoesntHavePassword(Guid linkedUserId)
        {
            var userLinked = _repositoryFacade.UserLinked(linkedUserId);

            if (!userLinked)
            {
                throw new UserDoesntHavePasswordException();
            }
        }

        private string GetUserPasswordHash(Guid userId)
        {
            var response = _repositoryFacade.GetByLinkedUserId(userId);

            return response.HashedPassword;
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return _passwordHasher.Verify(password, passwordHash);
        }

        private void ThrowIfVerificationFailed(bool verified)
        {
            if (!verified)
            {
                throw new IncorrectPasswordException();
            }
        }

        private async Task<string> GenerateJwt(Guid userId)
        {
            var userData = await GetUserData(userId);
            var userName = userData.Name ?? "";
            var userRole = userData.Role ?? "";

            var expires = GetDefaultExpiresTime();
            var token = _jwtProvider.GenerateToken(userId, userName, userRole, expires);

            return token;
        }

        private async Task<UserServiceUserModel> GetUserData(Guid userId)
        {
            var connector = _interserviceCommunicator.GetUserServiceConnector();
            var request = connector.CreateGetUserRequest(userId);
            var result = await request.Send();

            return result;
        }

        private DateTime GetDefaultExpiresTime() => DateTime.UtcNow.AddHours(1);

        private AuthenticationResponse CreateResponse(string token)
        {
            return new AuthenticationResponse(token);
        }
    }
}
