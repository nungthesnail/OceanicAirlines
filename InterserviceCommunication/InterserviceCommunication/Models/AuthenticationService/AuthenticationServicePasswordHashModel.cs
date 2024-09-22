namespace InterserviceCommunication.Models.AuthenticationService
{
    /// <summary>
    /// Модель хеша пароля пользователя
    /// </summary>
    public class AuthenticationServicePasswordHashModel
    {
        /// <summary>
        /// Уникальный идентификатор хеша пароля
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Хеш пароля
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Уникальный идентификатор пользователя, которому принадлежит хеш пароля
        /// </summary>
        public Guid? LinkedUserId { get; set; }
    }
}
