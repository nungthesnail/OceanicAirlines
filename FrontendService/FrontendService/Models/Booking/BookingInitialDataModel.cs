using System.ComponentModel.DataAnnotations;

namespace FrontendService.Models.Booking
{
	/// <summary>
	/// Модель данных, необходимых для корректной работы формы бронирования
	/// </summary>
	public class BookingInitialDataModel
	{
		/// <summary>
		/// Идентификатор запланированного рейса, на который совершается бронирование
		/// </summary>
		[Required]
		public int FlightId { get; set; }

		/// <summary>
		/// Количество пассажиров в бронировании
		/// </summary>
		[Required]
		public int PassengersCount { get; set; }
	}
}
