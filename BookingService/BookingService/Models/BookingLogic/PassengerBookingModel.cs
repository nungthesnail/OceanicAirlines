using EntityFrameworkLogic.Entities;


namespace BookingService.Models.BookingLogic
{
    /// <summary>
    /// Модель сущности связки пассажир-к-бронированию
    /// </summary>
    public class PassengerBookingModel
    {
        /// <summary>
        /// Идентификатор сущности связки
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Навигационное свойство, ссылающееся на сущность пассажира, которого связка связывает с бронированием
        /// </summary>
        public PassengerModel Passenger { get; set; } = null!;

        /// <summary>
        /// Максимальный вес ручной клади пассажира
        /// </summary>
        public float CarryOnMaxWeight { get; set; }

        /// <summary>
        /// Максимальный вес багажа пассажира
        /// </summary>
        public float BaggageMaxWeight { get; set; }

		/// <summary>
		/// Метод, строящий модель связки пассажир-к-бронированию из сущности связки. В будущем будет вынесен в отдельный строитель
		/// для разделения ответственности классов
		/// </summary>
		/// <param name="passengerToBooking">Сущность связки</param>
		/// <returns>Построенная модель связки</returns>
		public static PassengerBookingModel BuildFrom(PassengerToBooking passengerToBooking)
        {
            var passenger = PassengerModel.BuildFrom(passengerToBooking.Passenger);

            return new PassengerBookingModel
            {
                Id = passengerToBooking.Id,
                CarryOnMaxWeight = passengerToBooking.CarryOnMaxWeight,
                BaggageMaxWeight = passengerToBooking.BaggageMaxWeight,
                Passenger = passenger
            };
        }
    }
}
