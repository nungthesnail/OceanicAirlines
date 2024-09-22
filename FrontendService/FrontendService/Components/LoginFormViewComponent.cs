using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Components
{
	public class LoginFormViewComponent : ViewComponent
	{
		/// <summary>
		/// Компонент вида, отображающий форму авторизации
		/// </summary>
		/// <returns>Компонент вида</returns>
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
