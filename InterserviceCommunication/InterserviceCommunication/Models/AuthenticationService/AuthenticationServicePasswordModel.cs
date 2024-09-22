namespace InterserviceCommunication.Models.AuthenticationService
{
	/// <summary>
	/// Модель пароля пользователя
	/// </summary>
	public class AuthenticationServicePasswordModel
	{
		/// <summary>
		/// Уникальный идентификатор пользователя, которому принадлежит пароль
		/// </summary>
		public Guid LinkedUserId { get; set; }

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public string Password { get; set; } = null!;
	}
}
