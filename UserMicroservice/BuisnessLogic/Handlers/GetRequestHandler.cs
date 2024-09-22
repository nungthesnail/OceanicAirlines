using BuisnessLogic.Models;
using BuisnessLogic.Handlers.Exceptions;
using BuisnessLogic.Repository;
using BuisnessLogic.Repository.Exceptions;

namespace BuisnessLogic.Handlers
{
	/// <summary>
	/// Обработчик запроса получения пользователя. Проверяет корректность данных и делегирует получение репозиторию
	/// </summary>
	public class GetRequestHandler
    {
        private RepositoryFacade _repository;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="repository">Объект репозитория</param>
		public GetRequestHandler(RepositoryFacade repository)
        {
            _repository = repository;
        }

		/// <summary>
		/// Проверка корректности и делегирование получения пользователя в базе данных
		/// </summary>
		/// <param name="id">Уникальный идентификатор пользователя</param>
		/// <returns>Найденная сущность</returns>
		/// <exception cref="UserDoesntExistsException"></exception>
		public ResponseUserModel Handle(Guid id)
        {
            try
            {
                return GetUserById(id);
            }
            catch (UserNotFoundException)
            {
                throw new UserDoesntExistsException();
            }
        }

		/// <summary>
		/// Проверка корректности и делегирование получения пользователя в базе данных
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Найденная сущность</returns>
		/// <exception cref="UserDoesntExistsException"></exception>
		public ResponseUserModel Handle(string userName)
        {
            try
            {
                return GetUserByName(userName);
            }
            catch (UserNotFoundException)
            {
                throw new UserDoesntExistsException();
            }
        }

        private ResponseUserModel GetUserById(Guid id)
        {
            return _repository.GetById(id);
        }

        private ResponseUserModel GetUserByName(string userName)
        {
            return _repository.GetByName(userName);
        }
    }
}
