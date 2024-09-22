using BuisnessLogic.Models.Authentication;
using BuisnessLogic.Models.Management;
using BuisnessLogic.Api.Management;
using BuisnessLogic.Api.Authentication;

namespace BuisnessLogic.Api
{
    /// <summary>
    /// Класс API бизнес-логики сервиса аутентификации и авторизации. Агрегирует API авторизации и управления хешами паролей
    /// </summary>
    public class BuisnessLogicApi
    {
        private ManagementApi _managementApi;

        private AuthenticationApi _authenticationApi;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="managementApi">API управления хешами паролей</param>
        /// <param name="authenticationApi">API авторизации</param>
        public BuisnessLogicApi(ManagementApi managementApi, AuthenticationApi authenticationApi)
        {
            _managementApi = managementApi;
            _authenticationApi = authenticationApi;
        }

        /// <summary>
        /// Метод создания хеша пароля пользователя. Делегирует запрос API управления хешами
        /// </summary>
        /// <param name="request">Модель пароля пользователя</param>
        /// <returns>Созданный хеш пароля пользователя</returns>
        public async Task<ManagementResponse> Create(ManagementRequest request)
        {
            return await _managementApi.Create(request);
        }

        /// <summary>
        /// Метод получения хеша пароля пользователя. Делегирует запрос API управления хешами
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns>Найденный хеш пароля пользователя</returns>
        public ManagementResponse GetByUser(Guid userId)
        {
            return _managementApi.GetByUser(userId);
        }

        /// <summary>
        /// Метод обновления хеша пароля пользователя. Делегирует запрос API управления хешами
        /// </summary>
        /// <param name="request">Модель пароля пользователя</param>
        /// <returns>Обновленный хеш пароля пользователя</returns>
        public async Task<ManagementResponse> Update(ManagementRequest request)
        {
            return await _managementApi.Update(request);
        }

        /// <summary>
        /// Метод удаления хеша пароля пользователя. Делегирует запрос API управления хешами
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns>Удаленный хеш пароля пользователя</returns>
        public async Task<ManagementResponse> Delete(Guid userId)
        {
            return await _managementApi.Delete(userId);
        }

        /// <summary>
        /// Метод проверки существования пароля у пользователя. Делегирует запрос API управления хешами
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns>Удаленный хеш пароля пользователя</returns>
        public bool UserHasPassword(Guid userId)
        {
            return _managementApi.UserHasPassword(userId);
        }

        /// <summary>
        /// Метод прохождения процедуры аутентификации и авторизации. Делегирует запрос API авторизации
        /// </summary>
        /// <param name="request">Запрос авторизации</param>
        /// <returns>Результат авторизации в формате JSON Web Token</returns>
        public async Task<AuthenticationResponse> Authenticate(AuthenticationRequest request)
        {
            return await _authenticationApi.Authenticate(request);
        }
    }
}
