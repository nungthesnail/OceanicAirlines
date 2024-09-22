using FrontendService.ViewModels.Flights;


namespace FrontendService.ViewModels.Booking
{
	/// <summary>
	/// Модель вида, описывающая данные бронирования
	/// </summary>
	public class BookingViewModel
	{
		/// <summary>
		/// 10-значный буквенно-численный код бронирования
		/// </summary>
		public string Code { get; set; } = null!;

		/// <summary>
		/// Данные запланированного рейса, на который оформлено бронирование
		/// </summary>
		public FlightViewModel Flight { get; set; } = null!;

		/// <summary>
		/// Список пассажиров бронирования
		/// </summary>
		public IEnumerable<PassengerViewModel> Passengers { get; set; } = [];

		/// <summary>
		/// Статус подтверждения бронирования. Бронирование подтверждается после оплаты. В данный момент логика не реализована
		/// </summary>
		public bool Confirmed { get; set; } = false;
	}
}
