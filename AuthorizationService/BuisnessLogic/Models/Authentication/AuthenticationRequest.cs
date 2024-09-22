namespace BuisnessLogic.Models.Authentication
{
    /// <summary>
    /// Модель запроса аутентификации и авторизации
    /// </summary>
    public class AuthenticationRequest
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; } = String.Empty;
    }
}
