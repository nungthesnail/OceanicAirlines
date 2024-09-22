using BuisnessLogic.Models.Management;
using BuisnessLogic.Repository;

namespace BuisnessLogic.Handlers.Management
{
    /// <summary>
    /// Обработчик запроса обновления хеша пароля пользователя. Делегирует обновление репозиторию
    /// </summary>
    public class UpdateRequestHandler
    {
        private RepositoryFacade _repositoryFacade;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="repositoryFacade">Репозиторий</param>
        public UpdateRequestHandler(RepositoryFacade repositoryFacade)
        {
            _repositoryFacade = repositoryFacade;
        }

		/// <summary>
		/// Метод обработки запроса обновления хеша пароля пользователя. Делегирует обновление репозиторию
		/// </summary>
		/// <param name="request">Модель пароля пользователя</param>
		/// <returns>Обновленный хеш пароля пользователя</returns>
		public async Task<ManagementResponse> Handle(ManagementRequest request)
        {
            return await _repositoryFacade.Update(request);
        }
    }
}
