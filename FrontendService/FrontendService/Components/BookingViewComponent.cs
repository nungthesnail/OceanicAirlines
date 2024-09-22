using FrontendService.ViewModels.Booking;
using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Components
{
	/// <summary>
	/// Компонент вида, отображающий информацию о бронировании
	/// </summary>
	public class BookingViewComponent : ViewComponent
	{
		/// <summary>
		/// Отображает информацию о бронировании
		/// </summary>
		/// <param name="model">Модель вида бронирования</param>
		/// <returns>Компонент вида</returns>
		public IViewComponentResult Invoke(BookingViewModel model)
		{
			return View(model);
		}
	}
}
