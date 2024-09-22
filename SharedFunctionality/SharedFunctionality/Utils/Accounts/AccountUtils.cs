using System.Text.RegularExpressions;

namespace SharedFunctionality.Utils.Accounts
{
    /// <summary>
    /// Инструменты для работы с данными пользователей
    /// </summary>
    public static class AccountUtils
    {
        /// <summary>
        /// Паттерн регулярного выражения имени пользователя
        /// </summary>
        public const string _usernameRegexPattern = @"^[a-zA-Z_$][\w$]*$";

        /// <summary>
        /// Проверяет корректность имени пользователя
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <returns>Корректность имени пользователя</returns>
        public static bool IsValidName(string name)
        {
            var regex = new Regex(_usernameRegexPattern);
            
            var valid = regex.IsMatch(name);

            return valid;
        }
    }
}
