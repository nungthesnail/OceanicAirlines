using System.Net.Mail;

namespace SharedFunctionality.Utils.Email
{
    /// <summary>
    /// Инструменты для работы со значениями адресов электронной почты
    /// </summary>
    public static class EmailUtils
    {
		/// <summary>
		/// Проверяет корректность адреса электронной почты
		/// </summary>
		/// <param name="email">Адрес электронной почты</param>
		/// <returns>Корректность адреса электронной почты</returns>
		public static bool IsValidAddress(string email)
        {
            var result = MailAddress.TryCreate(email, out var _);

            return result;
        }
    }
}
