namespace InterserviceCommunication
{
    /// <summary>
    /// Настройки межсервисного связиста
    /// </summary>
    public class InterserviceCommunicatorSettings
    {
        /// <summary>
        /// Уникальный идентификатор, указывающий на пользователя межсервисного связиста
        /// </summary>
        public Guid ServiceId { get; set; }

        /// <summary>
        /// Пароль пользователя межсервисного связиста
        /// </summary>
        public string ServicePassword { get; set; } = String.Empty;

        /// <summary>
        /// Выполнять ли процедуру идентификации межсервисного связиста
        /// </summary>
        public bool DoAuthentication { get; set; } = true;

        /// <summary>
        /// Выполнять ли процедуру идентификации межсервисного связиста сразу после его создания
        /// </summary>
        public bool AuthenticateImmediately { get; set; } = true;
    }
}
