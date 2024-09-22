using BuisnessLogic.Models;
using BuisnessLogic.Handlers.Exceptions;
using BuisnessLogic.Repository;
using BuisnessLogic.Repository.Exceptions;

namespace BuisnessLogic.Handlers
{
	/// <summary>
	/// Обработчик запроса обновления пользователя. Проверяет корректность данных и делегирует обновление репозиторию
	/// </summary>
	public class UpdateRequestHandler
    {
        private RepositoryFacade _repository;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="repository">Объект репозитория</param>
		public UpdateRequestHandler(RepositoryFacade repository)
        {
            _repository = repository;
        }

		/// <summary>
		/// Проверка корректности и делегирование обновления пользователя в базе данных
		/// </summary>
		/// <param name="user">Модель пользователя из запроса</param>
		/// <returns>Обновленная сущность</returns>
		public async Task<ResponseUserModel> Handle(RequestUserModel user)
        {
            try
            {
                CheckUserExists(user);

                return await UpdateUser(user);
            }
            catch (UserNotFoundException)
            {
                throw new UserDoesntExistsException();
            }
        }

        private void CheckUserExists(RequestUserModel user)
        {
            if (!_repository.UserExists(user.Id))
            {
                throw new UserDoesntExistsException();
            }
        }

        private async Task<ResponseUserModel> UpdateUser(RequestUserModel user)
        {
            return await _repository.Update(user);
        }
    }
}
