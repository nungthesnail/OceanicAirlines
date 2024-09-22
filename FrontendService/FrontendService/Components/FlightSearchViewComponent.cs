using FrontendService.ViewModels.Flights;
using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Components
{
	/// <summary>
	/// Компонент вида, отображающий информацию о поиске запланированного рейса (критерии поиска)
	/// </summary>
	public class FlightSearchViewComponent : ViewComponent
	{
		/// <summary>
		/// Отображает информацию о поиске запланированного рейса (критерии поиска)
		/// </summary>
		/// <param name="model">Модель вида критериев поиска запланированного рейса</param>
		/// <returns>Компонент вида</returns>
		public IViewComponentResult Invoke(FlightSearchViewModel model)
		{
			return View(model);
		}
	}
}
