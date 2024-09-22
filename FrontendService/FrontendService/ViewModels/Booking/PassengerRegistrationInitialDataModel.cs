namespace FrontendService.ViewModels.Booking
{
	/// <summary>
	/// Модель вида, описывающие данные, необходимые для корректной работы формы бронирования
	/// </summary>
	public class PassengerRegistrationInitialDataModel
	{
		/// <summary>
		/// Идентификатор полета, на который оформляется бронирование
		/// </summary>
		public int FlightId { get; set; }

		/// <summary>
		/// Количество пассажиров в бронировании
		/// </summary>
		public int PassengersCount { get; set; }
	}
}
