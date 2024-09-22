using BuisnessLogic.Handlers.Authentication;
using BuisnessLogic.Handlers.Exceptions;
using BuisnessLogic.Models.Authentication;
using BuisnessLogic.Api.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using BuisnessLogic.Repository.Exceptions;

namespace BuisnessLogic.Api.Authentication
{
    /// <summary>
    /// Класс API бизнес-логики для аутентификации и авторизации
    /// </summary>
    public class AuthenticationApi
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов</param>
        public AuthenticationApi(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Метод прохождения процедуры аутентификации и авторизации
        /// </summary>
        /// <param name="request">Запрос аутентификации и авторизации</param>
        /// <returns>Результат авторизации, представленный в виде JSON Web Token</returns>
        /// <exception cref="UserDoesntExistsApiException"></exception>
        /// <exception cref="UserDoesntHavePasswordApiException"></exception>
        /// <exception cref="AuthenticationFailedApiException"></exception>
        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            try
            {
                var handler = _serviceProvider.GetService<AuthenticateRequestHandler>();

                return await handler!.Handle(request);
            }
            catch (UserDoesntExistsException)
            {
                throw new UserDoesntExistsApiException();
            }
            catch (UserDoesntHavePasswordException)
            {
                throw new UserDoesntHavePasswordApiException();
            }
            catch (UserDoesntLinkedException)
            {
                throw new UserDoesntHavePasswordApiException();
            }
            catch (IncorrectPasswordException)
            {
                throw new AuthenticationFailedApiException();
            }
        }
    }
}
