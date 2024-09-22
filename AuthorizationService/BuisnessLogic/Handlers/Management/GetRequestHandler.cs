using BuisnessLogic.Models.Management;
using BuisnessLogic.Repository;
using BuisnessLogic.Repository.Exceptions;
using BuisnessLogic.Handlers.Exceptions;

namespace BuisnessLogic.Handlers.Management
{
    /// <summary>
    /// Обработчик запроса получения хеша пароля пользователя. Делегирует получение репозиторию и обрабатывает случай отсутствия пароля у пользователя
    /// </summary>
    public class GetRequestHandler
    {
        private RepositoryFacade _repositoryFacade = null!;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="repositoryFacade">Репозиторий</param>
        public GetRequestHandler(RepositoryFacade repositoryFacade)
        {
            _repositoryFacade = repositoryFacade;
        }

		/// <summary>
		/// Метод обработки запроса получения хеша пароля пользователя. Делегирует получение репозиторию и обрабатывает случай отсутствия пароля у пользователя
		/// </summary>
		/// <param name="linkedUserId">Уникальный идентификатор пользователя</param>
		/// <returns>Найденный хеш пароля пользователя</returns>
		/// <exception cref="UserDoesntHavePasswordException"></exception>
		public ManagementResponse Handle(Guid linkedUserId)
        {
            try
            {
                return _repositoryFacade.GetByLinkedUserId(linkedUserId);
            }
            catch (UserDoesntLinkedException)
            {
                throw new UserDoesntHavePasswordException();
            }
        }
    }
}
