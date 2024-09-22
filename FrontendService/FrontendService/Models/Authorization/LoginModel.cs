namespace FrontendService.Models.Authorization
{
	/// <summary>
	/// Модель данных, необходимых для авторизации
	/// </summary>
	public class LoginModel
	{
		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Username { get; set; } = null!;

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public string Password { get; set; } = null!;
	}
}
