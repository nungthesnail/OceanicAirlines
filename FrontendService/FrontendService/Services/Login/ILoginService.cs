using FrontendService.Models.Authorization;
using FrontendService.Services.Login.Exceptions;

namespace FrontendService.Services.Login
{
	/// <summary>
	/// Интерфейс сервиса авторизации
	/// </summary>
	public interface ILoginService
	{
		/// <summary>
		/// Производит авторизацию пользователя
		/// </summary>
		/// <param name="model">Данные, необходимые для авторизации</param>
		/// <returns>JSON Web Token, представляющий результат авторизации</returns>
		/// <exception cref="LoginFailedException"></exception>
		public Task<string> Login(LoginModel model);
	}
}