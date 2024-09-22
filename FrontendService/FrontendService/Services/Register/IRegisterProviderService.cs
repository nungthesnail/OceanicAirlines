using FrontendService.Models.Authorization;
using FrontendService.Services.Register.Exceptions;

namespace FrontendService.Services.Register
{
	/// <summary>
	/// Интерфейс сервиса, осуществляющего регистрацию пользователя
	/// </summary>
	public interface IRegisterProviderService
	{
		/// <summary>
		/// Производит регистрацию пользователя
		/// </summary>
		/// <param name="model">Данные, необходимые для регистрации пользователя</param>
		/// <returns></returns>
		/// <exception cref="RegisterFailedException"></exception>
		public Task Register(RegisterModel model);
	}
}