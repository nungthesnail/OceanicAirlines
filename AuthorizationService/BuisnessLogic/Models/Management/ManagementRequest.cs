namespace BuisnessLogic.Models.Management
{
    /// <summary>
    /// Модель пароля пользователя
    /// </summary>
    public class ManagementRequest
    {
        /// <summary>
        /// Уникальный идентификатор хеша пароля
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор связанного пользователя
        /// </summary>
        public Guid LinkedUserId { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// Конструктор модели пароля пользователя
        /// </summary>
        /// <param name="id">Уникальный идентификатор хеша пароля</param>
        /// <param name="linkedUserId">Уникальный идентификатор связанного пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        public ManagementRequest(Guid id, Guid linkedUserId, string password)
        {
            Id = id;
            LinkedUserId = linkedUserId;
            Password = password;
        }
    }
}
