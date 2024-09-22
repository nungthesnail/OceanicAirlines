using BuisnessLogic.Models;
using BuisnessLogic.Handlers.Exceptions;
using BuisnessLogic.Repository;

namespace BuisnessLogic.Handlers
{
    /// <summary>
    /// Обработчик запроса создания пользователя. Проверяет корректность данных и делегирует создание репозиторию
    /// </summary>
    public class CreateRequestHandler
    {
        private RepositoryFacade _repository;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="repository">Объект репозитория</param>
        public CreateRequestHandler(RepositoryFacade repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Проверка корректности и делегирование создания пользователя в базе данных
        /// </summary>
        /// <param name="user">Модель пользователя из запроса</param>
        /// <returns>Созданная сущность</returns>
        public async Task<ResponseUserModel> Handle(RequestUserModel user)
        {
            CheckUserDoesntExist(user);

            var createdUser = await CreateUserInRepository(user);

            return createdUser;
        }

        private void CheckUserDoesntExist(RequestUserModel user)
        {
            if (_repository.UserExists(user.Id))
            {
                throw new UserAlreadyExistsException();
            }
        }

        private async Task<ResponseUserModel> CreateUserInRepository(RequestUserModel user)
        {
            return await _repository.Create(user);
        }
    }
}
