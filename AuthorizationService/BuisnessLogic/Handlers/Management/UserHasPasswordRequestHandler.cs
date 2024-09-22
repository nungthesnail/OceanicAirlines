using BuisnessLogic.Repository;

namespace BuisnessLogic.Handlers.Management
{
    /// <summary>
    /// Обработчик запроса проверки существования пароля у пользователя. Делегирует проверку репозиторию
    /// </summary>
    public class UserHasPasswordRequestHandler
    {
        private RepositoryFacade _repositoryFacade;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="repositoryFacade">Репозиторий</param>
        public UserHasPasswordRequestHandler(RepositoryFacade repositoryFacade)
        {
            _repositoryFacade = repositoryFacade;
        }

		/// <summary>
		/// Метод обработки запроса проверки существования пароля у пользователя. Делегирует проверку репозиторию
		/// </summary>
		/// <param name="linkedUserId">Уникальный идентификатор пользователя</param>
		/// <returns>Существование пароля у пользователя</returns>
		public bool Handle(Guid linkedUserId)
        {
            return _repositoryFacade.UserLinked(linkedUserId);
        }
    }
}
