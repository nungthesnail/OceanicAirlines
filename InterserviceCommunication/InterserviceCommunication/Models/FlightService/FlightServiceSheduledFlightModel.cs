namespace InterserviceCommunication.Models.FlightService
{
    /// <summary>
    /// Модель запланированного рейса
    /// </summary>
    public class FlightServiceSheduledFlightModel
    {
        /// <summary>
        /// Идентификатор запланированного рейса
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор базового рейса
        /// </summary>
        public int BaseFlightId { get; set; }

        /// <summary>
        /// Базовый рейс
        /// </summary>
        public FlightServiceBaseFlightModel? BaseFlight { get; set; }

        /// <summary>
        /// Статус выполнения запланированного рейса. В данный момент логика не реализована
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Время вылета
        /// </summary>
        public DateTime SheduledDeparture { get; set; }

        /// <summary>
        /// Время прилета
        /// </summary>
        public DateTime SheduledArrival { get; set; }

        /// <summary>
        /// Число, отражающее общее количество мест на борту самолета, которые можно забронировать.
        /// Никак не отражает количество забронированных мест
        /// </summary>
        public int SeatsCount { get; set; }
    }
}
