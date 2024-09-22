using FrontendService.ViewModels.Flights;
using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Components
{
	/// <summary>
	/// Компонент вида, отображающий информацию о запланированном рейсе
	/// </summary>
	public class FlightViewComponent : ViewComponent
	{
		/// <summary>
		/// Отображает информацию о запланированном рейсе
		/// </summary>
		/// <param name="model">Модель вида, описывающая запланированный рейс</param>
		/// <returns>Компонент вида</returns>
		public IViewComponentResult Invoke(FlightViewModel model)
		{
			return View(model);
		}
	}
}
