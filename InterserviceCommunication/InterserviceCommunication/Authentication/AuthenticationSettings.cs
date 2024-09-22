using InterserviceCommunication.Connectors;

namespace InterserviceCommunication.Authentication
{
    /// <summary>
    /// Настройки идентификации межсервисного связиста
    /// </summary>
    internal class AuthenticationSettings
    {
        /// <summary>
        /// Уникальный идентификатор, указывающий на пользователя межсервисного связиста
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Пароль пользователя межсервисного связиста
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Выполнять ли процедуру идентификации
        /// </summary>
        public bool DoAuthentication { get; set; } = true;

        /// <summary>
        /// Коннектор микросервиса аутентификации и авторизации
        /// </summary>
        public AuthenticationServiceConnector AuthenticationServiceConnector { get; set; } = null!;
    }
}
