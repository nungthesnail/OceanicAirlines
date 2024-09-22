namespace InterserviceCommunication.Models.FlightService
{
    /// <summary>
    /// Модель пары аэропортов
    /// </summary>
    public class FlightServiceAirportsPairModel
    {
        /// <summary>
        /// Идентификатор пары аэропортов
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор первого аэропорта
        /// </summary>
        public int FirstAirportId { get; set; }

        /// <summary>
        /// Первый аэропорт
        /// </summary>
        public FlightServiceAirportModel? FirstAirport { get; set; }

        /// <summary>
        /// Идентификатор второго аэропорта
        /// </summary>
        public int SecondAirportId { get; set; }

        /// <summary>
        /// Второй аэропорт
        /// </summary>
        public FlightServiceAirportModel? SecondAirport { get; set; }

        /// <summary>
        /// Расстояние между аэропортами в километровом масштабе
        /// </summary>
        public float DistanceInKm { get; set; }
    }
}
