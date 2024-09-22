namespace InterserviceCommunication.Models.FlightService
{
    /// <summary>
    /// Модель аэропорта
    /// </summary>
    public class FlightServiceAirportModel
    {
        /// <summary>
        /// Идентификатор аэропорта
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Код аэропорта IATA
        /// </summary>
        public string CodeIata { get; set; } = null!;

        /// <summary>
        /// Название аэропорта
        /// </summary>
        public string? Name { get; set; }
    }
}
