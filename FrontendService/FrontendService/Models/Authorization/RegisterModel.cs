namespace FrontendService.Models.Authorization
{
	/// <summary>
	/// Модель данных, необходимых для регистрации пользователя
	/// </summary>
	public class RegisterModel
	{
		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Username { get; set; } = null!;

		/// <summary>
		/// Адрес электронной почты пользователя
		/// </summary>
		public string Email { get; set; } = null!;

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public string Password { get; set; } = null!;

		/// <summary>
		/// Повторение пароля
		/// </summary>
		public string PasswordRepeat { get; set; } = null!;
	}
}
