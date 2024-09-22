namespace EntityFrameworkLogic.Entities
{
    /// <summary>
    /// Сущность связки пассажир-к-бронированию
    /// </summary>
    public class PassengerToBooking
    {
		/// <summary>
		/// Идентификатор сущности связки
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Внешний ключ, ссылающийся на идентификатор сущности пассажира, которого связка связывает с бронированием
		/// </summary>
		public int PassengerId { get; set; }

		/// <summary>
		/// Навигационное свойство, ссылающееся на сущность пассажира, которого связка связывает с бронированием
		/// </summary>
		public Passenger Passenger { get; set; } = null!;

		/// <summary>
		/// Внешний ключ, ссылающ1еся на бронирование, с которым связка связывает пассажира
		/// </summary>
		public int BookingId { get; set; }

		/// <summary>
		/// Навигационное свойство, ссылающееся на бронирование, с которым связка связывает пассажира
		/// </summary>
		public Booking Booking { get; set; } = null!;

		/// <summary>
		/// Максимальный вес ручной клади пассажира
		/// </summary>
		public float CarryOnMaxWeight { get; set; }

		/// <summary>
		/// Максимальный вес багажа пассажира
		/// </summary>
		public float BaggageMaxWeight { get; set; } = 0f;
    }
}
