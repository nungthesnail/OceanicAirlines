namespace EntityFrameworkLogic
{
    /// <summary>
    /// Сущность пользователя в базе данных
    /// </summary>
    public class User
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Адрес электронной почты пользователя
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Время создания пользователя
        /// </summary>
        public DateTime? CreatedAt { get; set; }
    }
}
