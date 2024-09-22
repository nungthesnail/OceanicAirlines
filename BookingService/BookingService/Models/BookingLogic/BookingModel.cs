using EntityFrameworkLogic.Entities;

namespace BookingService.Models.BookingLogic
{
    /// <summary>
    /// Модель, описывающая сущность бронирования
    /// </summary>
    public class BookingModel
    {
        /// <summary>
        /// Идентификатор бронирования
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Навигационное свойство, указывающее на все связки пассажир-к-бронированию, в которых в роли бронирования выступает данное
        /// </summary>
        public List<PassengerBookingModel> Passengers { get; set; } = [];

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
        /// Метод, строящий модель бронирования из сущности бронирования. В будущем будет вынесен в отдельный строитель
        /// для разделения ответственности классов
        /// </summary>
        /// <param name="booking">Сущность бронирования</param>
        /// <returns>Построенная модель бронирования</returns>
        public static BookingModel BuildFrom(Booking booking)
        {
            var passengersToBookings = from pb in booking.PassengersToBookings
                                       select PassengerBookingModel.BuildFrom(pb);

            return new BookingModel
            {
                Id = booking.Id,
                FlightId = booking.FlightId,
                Code = booking.Code,
                CustomerUserId = booking.CustomerUserId,
                Confirmed = booking.Confirmed,
                CreatedAt = booking.CreatedAt,
                Passengers = passengersToBookings.ToList()
            };
        }
    }
}
