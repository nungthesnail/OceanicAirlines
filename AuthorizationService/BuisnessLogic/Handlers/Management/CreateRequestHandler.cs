using BuisnessLogic.Handlers.Exceptions;
using BuisnessLogic.Models.Management;
using BuisnessLogic.Repository;
using InterserviceCommunication;
using InterserviceCommunication.Exceptions;

namespace BuisnessLogic.Handlers.Management
{
    /// <summary>
    /// Обработчик запроса создания хеша пароля пользователя. Делегирует создание репозиторию
    /// </summary>
    public class CreateRequestHandler
    {
        private RepositoryFacade _repositoryFacade;

        private InterserviceCommunicator _interserviceCommunicator;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="repositoryFacade">Репозиторий</param>
        /// <param name="interserviceCommunicator">Межсервисный связист</param>
        public CreateRequestHandler(RepositoryFacade repositoryFacade, InterserviceCommunicator interserviceCommunicator)
        {
            _repositoryFacade = repositoryFacade;
            _interserviceCommunicator = interserviceCommunicator;
        }

        /// <summary>
        /// Метод обработки запроса создания хеша пароля пользователя. Проверяет существование пользователя и делегирует создание хеша репозиторию.
        /// </summary>
        /// <param name="request">Модель пароля пользователя</param>
        /// <returns>Созданный хеш пароля пользователя</returns>
        public async Task<ManagementResponse> Handle(ManagementRequest request)
        {
            await ThrowIfUserDoesntExists(request.LinkedUserId);

            return await _repositoryFacade.Create(request);
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
    }
}
