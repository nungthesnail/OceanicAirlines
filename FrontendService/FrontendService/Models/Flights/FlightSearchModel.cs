using System.ComponentModel.DataAnnotations;

namespace FrontendService.Models.Flights
{
    /// <summary>
    /// Модель критериев поиска запланированного рейса
    /// </summary>
    public class FlightSearchModel
    {
        /// <summary>
        /// Код аэропорта вылета IATA
        /// </summary>
        [Required]
        public string DepartureAirport { get; set; } = null!;

        /// <summary>
        /// Код аэропорта прилета IATA
        /// </summary>
        [Required]
        public string ArrivalAirport { get; set; } = null!;

        /// <summary>
        /// Дата вылета
        /// </summary>
        [Required]
        public DateOnly DepartureDate { get; set; }

        /// <summary>
        /// Количество пассажиров
        /// </summary>
        [Required]
        public int PassengerCount { get; set; }
    }
}
