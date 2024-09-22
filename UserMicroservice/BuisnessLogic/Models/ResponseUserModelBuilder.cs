using System.Text.Json;
using BuisnessLogic.Models.Exceptions;

namespace BuisnessLogic.Models
{
	/// <summary>
	/// Строитель модели пользователя из ответа
	/// </summary>
	public class ResponseUserModelBuilder
    {
        private Guid _id;

        private string? _name;

        private string? _email;

        private string? _role;

        private DateTime _createdAt;

		/// <summary>
		/// Конструктор без параметров
		/// </summary>
		public ResponseUserModelBuilder()
        { }

		/// <summary>
		/// Метод построения модели пользователя из ответа
		/// </summary>
		/// <returns>Построенная модель пользователя</returns>
		/// <exception cref="ModelBuildingException"></exception>
		public ResponseUserModel Build()
        {
            if (_name == null || _email == null)
            {
                throw new ModelBuildingException();
            }

            return new ResponseUserModel(_id, _name, _email, _role, _createdAt);
        }

		/// <summary>
		/// Метод построения модели пользователя из строки в формате JSON
		/// </summary>
		/// <param name="json">Строка в формате JSON</param>
		/// <returns>Построенная модель пользователя</returns>
		/// <exception cref="ModelBuildingException"></exception>
		public ResponseUserModel BuildFromJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ResponseUserModel>(json)!;
            }
            catch (JsonException)
            {
                throw new ModelBuildingException();
            }
        }

		/// <summary>
		/// Установка идентификатора пользователя
		/// </summary>
		/// <param name="id">Уникальный идентификатор пользователя</param>
		public void SetId(Guid id)
        {
            _id = id;
        }

		/// <summary>
		/// Установка имени пользователя
		/// </summary>
		/// <param name="name">Имя пользователя</param>
		public void SetName(string name)
        {
            _name = name;
        }

		/// <summary>
		/// Установка адреса электронной почты пользователя
		/// </summary>
		/// <param name="email">Адрес электронной почты пользователя</param>
		public void SetEmail(string email)
        {
            _email = email;
        }

		/// <summary>
		/// Установка роли пользователя
		/// </summary>
		/// <param name="role">Роль пользователя</param>
		public void SetRole(string role)
        {
            _role = role;
        }

		/// <summary>
		/// Установка времени создания пользователя
		/// </summary>
		/// <param name="createdAt">Время создания пользователя</param>
		public void SetCreatedAt(DateTime createdAt)
        {
            _createdAt = createdAt;
        }
    }
}
