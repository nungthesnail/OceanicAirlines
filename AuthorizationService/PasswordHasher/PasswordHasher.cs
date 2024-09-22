namespace PasswordUtils
{
    /// <summary>
    /// Сервис хеширования пароля
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Количество итераций хеширования алгоритма BCrypt
        /// </summary>
        private const int _iterationCount = 11;

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public PasswordHasher()
        { }

        public string Generate(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, _iterationCount);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
        }
    }
}
