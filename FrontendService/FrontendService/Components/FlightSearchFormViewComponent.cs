using Microsoft.AspNetCore.Mvc;

namespace FrontendService.ViewComponents
{
	/// <summary>
	/// Компонент вида, отображающий форму поиска запланированных рейсов
	/// </summary>
	public class FlightSearchFormViewComponent : ViewComponent
	{
		/// <summary>
		/// Отображает форму поиска запланированных рейсов
		/// </summary>
		/// <returns>Компонент вида</returns>
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}
