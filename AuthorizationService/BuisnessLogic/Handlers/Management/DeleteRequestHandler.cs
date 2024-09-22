using BuisnessLogic.Models.Management;
using BuisnessLogic.Repository;

namespace BuisnessLogic.Handlers.Management
{
    /// <summary>
    /// Обработчик запроса удаления хеша пароля пользователя. Делегирует удаление репозиторию
    /// </summary>
    public class DeleteRequestHandler
    {
        private RepositoryFacade _repositoryFacade;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="repositoryFacade">Репозиторий</param>
        public DeleteRequestHandler(RepositoryFacade repositoryFacade)
        {
            _repositoryFacade = repositoryFacade;
        }

        /// <summary>
        /// Метод обработки запроса удаления. Делегирует удаление репозиторию
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns>Удаленный хеш пароля пользователя</returns>
        public async Task<ManagementResponse> Handle(Guid userId)
        {
            return await _repositoryFacade.DeleteByLinkedUserId(userId);
        }
    }
}
