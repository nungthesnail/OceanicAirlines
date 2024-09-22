namespace EntityFrameworkLogic
{
    /// <summary>
    /// Сущность хеша пароля пользователя в базе данных
    /// </summary>
    public class PasswordHash
    {
        /// <summary>
        /// Уникальный идентификатор хеша пароля пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор связанного пользователя
        /// </summary>
        public Guid LinkedUserId { get; set; }

        /// <summary>
        /// Хеш пароля пользователя
        /// </summary>
        public string HashedPassword { get; set; } = null!;
    }
}
