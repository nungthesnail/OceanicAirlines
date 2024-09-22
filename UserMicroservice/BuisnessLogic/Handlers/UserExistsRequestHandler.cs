using BuisnessLogic.Models;
using BuisnessLogic.Repository;
using EntityFrameworkLogic;

namespace BuisnessLogic.Handlers
{
	/// <summary>
	/// Обработчик запроса проверки существования пользователя. Проверяет корректность данных и делегирует проверку репозиторию
	/// </summary>
	public class UserExistsRequestHandler
    {
        private RepositoryFacade _repository = null!;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="repository">Объект репозитория</param>
		public UserExistsRequestHandler(RepositoryFacade repository)
        {
            _repository = repository;
        }

		/// <summary>
		/// Делегирование проверки существования пользователя в базе данных
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Существование пользователя</returns>
		public BoolResponseModel Handle(Guid userId)
        {
            var exists = _repository.UserExists(userId);

            var response = CreateBoolResponse(exists);

            return response;
        }

		/// <summary>
		/// Делегирование проверки существования пользователя в базе данных
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Существование пользователя</returns>
		public BoolResponseModel Handle(string userName)
        {
            var exists = _repository.UserExists(userName);

            var response = CreateBoolResponse(exists);

            return response;
        }

        private BoolResponseModel CreateBoolResponse(bool value)
        {
            return new BoolResponseModel()
            {
                Result = value
            };
        }
    }
}
