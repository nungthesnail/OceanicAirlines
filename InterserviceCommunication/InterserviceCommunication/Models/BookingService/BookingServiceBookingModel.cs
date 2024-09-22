namespace InterserviceCommunication.Models.BookingService
{
	/// <summary>
	/// Модель бронирования
	/// </summary>
	public class BookingServiceBookingModel
	{
		/// <summary>
		/// Идентификатор бронирования
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Пассажиры бронирования
		/// </summary>
		public IEnumerable<BookingServicePassengerToBookingModel> Passengers { get; set; } = [];

		/// <summary>
		/// Идентификатор запланированного рейса, на который оформлено бронирование
		/// </summary>
		public int FlightId { get; set; }

		/// <summary>
		/// 10-значный буквенно-числовой код бронирования
		/// </summary>
		public string Code { get; set; } = null!;

		/// <summary>
		/// Уникальный идентификатор пользователя, оформившего бронирование
		/// </summary>
		public Guid CustomerUserId { get; set; }

		/// <summary>
		/// Статус подтверждения бронирования. Бронирование становится подтвержденным после оплаты. В данный момент логика не реализована
		/// </summary>
		public bool Confirmed { get; set; } = false;

		/// <summary>
		/// Время бронирования
		/// </summary>
		public DateTime CreatedAt { get; set; }
	}
}
