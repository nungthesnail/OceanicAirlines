namespace InterserviceCommunication.Models.BookingService
{
	/// <summary>
	/// Модель связки пассажир-к-бронированию
	/// </summary>
	public class BookingServicePassengerToBookingModel
	{
		/// <summary>
		/// Идентификатор связки
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Пассажир
		/// </summary>
		public BookingServicePassengerModel Passenger { get; set; } = null!;
		
		/// <summary>
		/// Максимальный вес ручной клади пассажира
		/// </summary>
		public float MaxCarryOnWeight { get; set; } = 10;

		/// <summary>
		/// Максимальный вес багажа пассажира
		/// </summary>
		public float MaxBaggageWeight { get; set; } = 10;
	}
}
