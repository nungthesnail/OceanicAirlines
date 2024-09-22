using BuisnessLogic.Handlers.Exceptions;
using BuisnessLogic.Handlers.Management;
using BuisnessLogic.Models.Management;
using BuisnessLogic.Api.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using BuisnessLogic.Repository.Exceptions;

namespace BuisnessLogic.Api.Management
{
    /// <summary>
    /// Класс API бизнес-логики по управлению хешами паролей пользователей в базе данных
    /// </summary>
    public class ManagementApi
    {
        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="serviceProvider">Провайдер сервисов</param>
        public ManagementApi(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Метод для создания хеша пароля пользователя. Делегирует создание обработчику запроса
        /// </summary>
        /// <param name="request">Модель пароля пользователя</param>
        /// <returns>Модель созданного хеша пароля пользователя</returns>
        /// <exception cref="UserDoesntExistsApiException"></exception>
        /// <exception cref="UserAlreadyHasPasswordApiException"></exception>
        public async Task<ManagementResponse> Create(ManagementRequest request)
        {
            try
            {
                var handler = _serviceProvider.GetService<CreateRequestHandler>();

                return await handler!.Handle(request);
            }
            catch (UserDoesntExistsException)
            {
                throw new UserDoesntExistsApiException();
            }
            catch (UserAlreadyLinkedException)
            {
                throw new UserAlreadyHasPasswordApiException();
            }
        }

        /// <summary>
        /// Метод получения хеша пароля пользователя. Делегирует получение обработчику запроса
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns>Найденный хеш пароля пользователя</returns>
        /// <exception cref="UserDoesntExistsApiException"></exception>
        /// <exception cref="UserDoesntHavePasswordApiException"></exception>
        public ManagementResponse GetByUser(Guid userId)
        {
            try
            {
                var handler = _serviceProvider.GetService<GetRequestHandler>();

                return handler!.Handle(userId);
            }
            catch (UserDoesntExistsException)
            {
                throw new UserDoesntExistsApiException();
            }
            catch (UserDoesntHavePasswordException)
            {
                throw new UserDoesntHavePasswordApiException();
            }
        }

        /// <summary>
        /// Метод обновления хеша пароля пользователя. Делегирует обновление обработчику запроса
        /// </summary>
        /// <param name="request">Модель пароля пользователя</param>
        /// <returns>Обновленный хеш пароля пользователя</returns>
        /// <exception cref="BadRequestApiException"></exception>
        public async Task<ManagementResponse> Update(ManagementRequest request)
        {
            try
            {
                var handler = _serviceProvider.GetService<UpdateRequestHandler>();

                return await handler!.Handle(request);
            }
            catch (PasswordHashDoesntExistsException)
            {
                throw new BadRequestApiException();
            }
        }

        /// <summary>
        /// Метод удаления хеша пароля пользователя. Делегирует удаление обработчику запроса
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns></returns>
        /// <exception cref="UserDoesntHavePasswordApiException"></exception>
        public async Task<ManagementResponse> Delete(Guid userId)
        {
            try
            {
                var handler = _serviceProvider.GetService<DeleteRequestHandler>();

                return await handler!.Handle(userId);
            }
            catch (UserDoesntLinkedException)
            {
                throw new UserDoesntHavePasswordApiException();
            }
        }

        /// <summary>
        /// Метод проверки существования у пользователя пароля. Делегирует проверку обработчику запроса
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns>Существование пароля у пользователя</returns>
        public bool UserHasPassword(Guid userId)
        {
            var handler = _serviceProvider.GetService<UserHasPasswordRequestHandler>();

            return handler!.Handle(userId);
        }
    }
}
