using BuisnessLogic.Models;
using BuisnessLogic.Handlers.Exceptions;
using BuisnessLogic.Repository;
using BuisnessLogic.Repository.Exceptions;

namespace BuisnessLogic.Handlers
{
	/// <summary>
	/// Обработчик запроса удаления пользователя. Проверяет корректность данных и делегирует удаление репозиторию
	/// </summary>
	public class DeleteRequestHandler
    {
        private RepositoryFacade _repository;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="repository">Объект репозитория</param>
		public DeleteRequestHandler(RepositoryFacade repository)
        {
            _repository = repository;
        }

		/// <summary>
		/// Проверка корректности и делегирование удаления пользователя в базе данных
		/// </summary>
		/// <param name="id">Уникальный идентификатор пользователя</param>
		/// <returns>Удаленная сущность</returns>
		public async Task<ResponseUserModel> Handle(Guid id)
        {
            try
            {
                return await DeleteUser(id);
            }
            catch (UserNotFoundException)
            {
                throw new UserDoesntExistsException();
            }
        }

		/// <summary>
		/// Проверка корректности и делегирование удаления пользователя в базе данных
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Удаленная сущность</returns>
		public async Task<ResponseUserModel> Handle(string userName)
        {
            try
            {
                return await DeleteUser(userName);
            }
            catch (UserNotFoundException)
            {
                throw new UserDoesntExistsException();
            }
        }

        private async Task<ResponseUserModel> DeleteUser(Guid id)
        {
            return await _repository.DeleteById(id);
        }

        private async Task<ResponseUserModel> DeleteUser(string userName)
        {
            return await _repository.DeleteByName(userName);
        }
    }
}
