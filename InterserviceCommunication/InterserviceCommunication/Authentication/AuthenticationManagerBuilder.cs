namespace InterserviceCommunication.Authentication
{
    /// <summary>
    /// Строитель менеджера идентификации
    /// </summary>
    internal static class AuthenticationManagerBuilder
    {
        /// <summary>
        /// Строит менеджер идентификации
        /// </summary>
        /// <param name="settings">Настройки идентификации</param>
        /// <returns>Построенный менеджер идентификации</returns>
        public static AuthenticationManager Build(AuthenticationSettings settings)
        {
            return AuthenticationManager.CreateAuthenticationManager(settings);
        }
    }
}
