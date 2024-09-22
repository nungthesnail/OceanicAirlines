using FrontendService.ViewModels.Booking;
using Microsoft.AspNetCore.Mvc;


namespace FrontendService.Components
{
	/// <summary>
	/// Компонент вида, отображающий форму внесения данных пассажиров в бронировании
	/// </summary>
	public class PassengerRegistrationFormViewComponent : ViewComponent
	{
		/// <summary>
		/// Отображает форму внесения данных пассажиров в бронировании
		/// </summary>
		/// <param name="model">Данные о бронировании</param>
		/// <returns>Компонент вида</returns>
		public IViewComponentResult Invoke(PassengerRegistrationInitialDataModel model)
		{
			return View(model);
		}
	}
}
