namespace InterserviceCommunication.Models.AuthenticationService
{
    /// <summary>
    /// Модель данных, необходимых для прохождения процедуры аутентификации и авторизации
    /// </summary>
    public class AuthenticationServiceAuthenticationModel
    {
        /// <summary>
        /// Уникальный идентификатор пользователя
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string? Password { get; set; }
    }
}
