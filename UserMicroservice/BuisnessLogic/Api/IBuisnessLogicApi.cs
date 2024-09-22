using BuisnessLogic.Models;

namespace BuisnessLogic.Api
{
    /// <summary>
    /// API для взаимодействия с бизнес-логикой сервиса пользователей
    /// </summary>
    public interface IBuisnessLogicApi
    {
        /// <summary>
        /// Создание сущности пользователя в базе данных и возврат созданной сущности
        /// </summary>
        /// <param name="source">Модель пользователя из запроса</param>
        /// <returns>Созданная сущность</returns>
        public Task<ResponseUserModel> Create(RequestUserModel source);

		/// <summary>
		/// Обновление сущности пользователя в базе данных и возврат созданной сущности
		/// </summary>
		/// <param name="source">Модель пользователя из запроса</param>
		/// <returns>Обновленная сущность</returns>
		public Task<ResponseUserModel> Update(RequestUserModel source);

		/// <summary>
		/// Удаление сущности пользователя пользователя в базе данных и возврат удаленной сущности
		/// </summary>
		/// <param name="id">Уникальный идентификатор пользователя</param>
		/// <returns>Удаленная сущность</returns>
		public Task<ResponseUserModel> Delete(Guid id);

		/// <summary>
		/// Удаление сущности пользователя пользователя в базе данных и возврат удаленной сущности
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Удаленная сущность</returns>
		public Task<ResponseUserModel> Delete(string userName);

		/// <summary>
		/// Получение сущности пользователя пользователя из базы данных
		/// </summary>
		/// <param name="id">Уникальный идентификатор пользователя</param>
		/// <returns>Найденная сущность</returns>
		public ResponseUserModel Get(Guid id);

		/// <summary>
		/// Получение сущности пользователя пользователя из базы данных
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Найденная сущность</returns>
		public ResponseUserModel Get(string userName);

		/// <summary>
		/// Проверка существования пользователя с указанным идентификатором
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Существование пользователя</returns>
		public BoolResponseModel UserExists(Guid userId);

		/// <summary>
		/// Проверка существования пользователя с указанным именем
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Существование пользователя</returns>
		public BoolResponseModel UserExists(string userName);
	}
}
