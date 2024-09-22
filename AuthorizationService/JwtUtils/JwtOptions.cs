using Microsoft.Extensions.Configuration;

namespace JwtUtils
{
    /// <summary>
    /// Настройки сервиса JSON Web Token'ов
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Секретный ключ
        /// </summary>
        public readonly string Secret;

        /// <summary>
        /// Конструктор настроек
        /// </summary>
        /// <param name="config">Конфигурация приложения</param>
        public JwtOptions(IConfiguration config)
        {
            Secret = config["ServiceSettings:SecretKey"] ?? "";
        }
    }
}
