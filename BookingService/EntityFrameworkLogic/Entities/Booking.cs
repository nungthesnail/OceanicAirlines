namespace EntityFrameworkLogic.Entities
{
    /// <summary>
    /// Сущность бронирования
    /// </summary>
    public class Booking
    {
		/// <summary>
		/// Идентификатор бронирования
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Идентификатор запланированного полета
		/// </summary>
		public int FlightId { get; set; }

		/// <summary>
		/// 10-значный буквенно-числовой код бронирования
		/// </summary>
		public string Code { get; set; } = null!;

		/// <summary>
		/// Уникальный идентификатор пользователя, совершившего бронирование
		/// </summary>
		public Guid CustomerUserId { get; set; }

		/// <summary>
		/// Подтверждение бронирования. Становится подтвержденным после оплаты. Логика в данный момент еще не реализована
		/// </summary>
		public bool Confirmed { get; set; } = false;

		/// <summary>
		/// Время бронирования
		/// </summary>
		public DateTime? CreatedAt { get; set; } = null;

		/// <summary>
		/// Навигационное свойство, указывающее на все связки пассажир-к-бронированию, в которых в роли бронирования выступает данное
		/// </summary>
		public List<PassengerToBooking> PassengersToBookings { get; set; } = [];
    }
}
