namespace BuisnessLogic.Models.Authentication
{
    /// <summary>
    /// Модель результата авторизации, содержащая JSON Web Token
    /// </summary>
    public class AuthenticationResponse
    {
        /// <summary>
        /// Результат авторизации в формате JSON Web Token
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Конструктор модели результата авторизации
        /// </summary>
        /// <param name="token">JSON Web Token</param>
        public AuthenticationResponse(string token)
        {
            Result = token;
        }
    }
}
