namespace InterserviceCommunication.Models.UserService
{
    /// <summary>
    /// Модель пользователя
    /// </summary>
    public class UserServiceUserModel
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// Адрес электронной почты пользователя
        /// </summary>
        public string? Email { get; set; } = null;

        /// <summary>
        /// Время создания пользователя
        /// </summary>
        public DateTime? CreatedAt { get; set; } = null;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public string? Role {  get; set; } = null;
    }
}
