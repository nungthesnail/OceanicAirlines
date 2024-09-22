using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Components
{
	/// <summary>
	/// Компонент вида, отображающий шапку сайта
	/// </summary>
	public class HeaderViewComponent : ViewComponent
	{
		/// <summary>
		/// Отображает шапку сайта
		/// </summary>
		/// <returns>Компонент вида</returns>
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
