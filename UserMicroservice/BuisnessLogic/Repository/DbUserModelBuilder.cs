using BuisnessLogic.Models;
using EntityFrameworkLogic;

namespace BuisnessLogic.Repository
{
    /// <summary>
    /// Конвертер сущности пользователя и модели запроса/ответа
    /// </summary>
    public static class DbUserModelBuilder
    {
        /// <summary>
        /// Метод построения сущности пользователя из модели запроса
        /// </summary>
        /// <param name="source">Модель пользователя из запроса</param>
        /// <returns>Построенная сущность</returns>
        public static User FromRequestUserModel(RequestUserModel source)
        {
            var result = new User
            {
                Id = source.Id,
                Name = source.Name,
                Email = source.Email,
                Role = source.Role
            };

            return result;
        }

		/// <summary>
		/// Метод построения сущности пользователя из модели ответа
		/// </summary>
		/// <param name="source">Модель пользователя из ответа</param>
		/// <returns>Построенная сущность</returns>
		public static User FromResponseUserModel(ResponseUserModel source)
        {
            var result = new User
            {
                Id = source.Id,
                Name = source.Name,
                Email = source.Email,
                Role = source.Role,
                CreatedAt = source.CreatedAt
            };

            return result;
        }

        /// <summary>
        /// Метод построения пользователя из модели ответа из сущности пользователя
        /// </summary>
        /// <param name="source">Сущность пользователя</param>
        /// <returns>Построенная модель пользователя из ответа</returns>
        public static ResponseUserModel DbUserModelToResponseUserModel(User source)
        {
            var result = new ResponseUserModel()
            {
                Id = source.Id,
                Name = source.Name ?? "",
                Email = source.Email ?? "",
                Role = source.Role,
                CreatedAt = source.CreatedAt
            };

            return result;
        }
    }
}
