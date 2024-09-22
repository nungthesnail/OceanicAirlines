namespace BuisnessLogic.Models
{
	/// <summary>
	/// Модель пользователя из ответа
	/// </summary>
	public class ResponseUserModel
    {
		/// <summary>
		/// Уникальный идентификатор пользователя
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Name { get; set; } = null!;

		/// <summary>
		/// Адрес электронной почты пользователя
		/// </summary>
		public string Email { get; set; } = null!;

		/// <summary>
		/// Роль пользователя
		/// </summary>
		public string? Role { get; set; }

		/// <summary>
		/// Время создания пользователя
		/// </summary>
		public DateTime? CreatedAt { get; set; }

		/// <summary>
		/// Конструктор модели
		/// </summary>
		/// <param name="id">Уникальный идентификатор пользователя</param>
		/// <param name="name">Имя пользователя</param>
		/// <param name="email">Адрес электронной почты пользователя</param>
		/// <param name="role">Роль пользователя</param>
		/// <param name="createdAt">Время создания пользователя</param>
		public ResponseUserModel(
            Guid id,
            string name,
            string email,
            string? role = null,
            DateTime? createdAt = null)
        {
            Id = id;
            Name = name;
            Email = email;
            Role = role;
            CreatedAt = createdAt;
        }

		/// <summary>
		/// Конструктор без параметров
		/// </summary>
        public ResponseUserModel()
        { }
    }
}
