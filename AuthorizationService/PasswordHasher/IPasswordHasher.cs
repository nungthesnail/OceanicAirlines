namespace PasswordUtils
{
    /// <summary>
    /// Интерфейс сервиса хеширования пароля
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Создает хеш пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>Хеш пароля</returns>
        public string Generate(string password);

        /// <summary>
        /// Проверяет пароль на верность
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="hash">Хеш пароля</param>
        /// <returns>Верность пароля</returns>
        public bool Verify(string password, string hash);
    }
}
