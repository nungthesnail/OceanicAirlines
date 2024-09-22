namespace BuisnessLogic.Models.Management
{
    /// <summary>
    /// Модель хеша пароля пользователя
    /// </summary>
    public class ManagementResponse
    {
        /// <summary>
        /// Уникальный идентификатор пароля
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор связанного пользователя
        /// </summary>
        public Guid LinkedUserId { get; set; }

        /// <summary>
        /// Хеш пароля
        /// </summary>
        public string HashedPassword { get; set; } = null!;

		/// <summary>
		/// Конструктор модели хеша пароля пользователя
		/// </summary>
		/// <param name="id">Уникальный идентификатор пароля</param>
		/// <param name="linkedUserId">Уникальный идентификатор связанного пользователя</param>
		/// <param name="hashedPassword">Хеш пароля</param>
		public ManagementResponse(Guid id, Guid linkedUserId, string hashedPassword)
        {
            Id = id;
            LinkedUserId = linkedUserId;
            HashedPassword = hashedPassword;
        }
    }
}
