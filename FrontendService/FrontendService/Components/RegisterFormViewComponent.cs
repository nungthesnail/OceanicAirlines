using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Components
{
	/// <summary>
	/// Компонент вида, отображающий форму регистрации пользователя
	/// </summary>
	public class RegisterFormViewComponent : ViewComponent
	{
		/// <summary>
		/// Отображает форму регистрации пользователя
		/// </summary>
		/// <returns>Компонент вида</returns>
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
