namespace InterserviceCommunication.Models.FlightService
{
    /// <summary>
    /// Модель базового рейса
    /// </summary>
    public class FlightServiceBaseFlightModel
    {
        /// <summary>
        /// Идентификатор базового рейса
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор пары аэропортов
        /// </summary>
        public int AirportsPairId { get; set; }

        /// <summary>
        /// Пара аэропортов
        /// </summary>
        public FlightServiceAirportsPairModel? AirportsPair { get; set; }

        /// <summary>
        /// Тип воздушного судна
        /// </summary>
        public string? AircraftType { get; set; }
    }
}
