using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Components
{
	/// <summary>
	/// Компонент вида, отображающий подвал сайта
	/// </summary>
	public class FooterViewComponent : ViewComponent
	{
		/// <summary>
		/// Отображает подвал сайта
		/// </summary>
		/// <returns>Компонент вида</returns>
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
