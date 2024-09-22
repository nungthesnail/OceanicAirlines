namespace FlightService.Models.Search
{
	/// <summary>
	/// Модель критериев поиска запланированных рейсов
	/// </summary>
    public class FlightSearchModel
    {
		/// <summary>
		/// Код аэропорта вылета IATA
		/// </summary>
        public string? DepartureAirport { get; set; } = null;

		/// <summary>
		/// Код аэропорта прилета IATA
		/// </summary>
        public string? ArrivalAirport { get; set; } = null;

		/// <summary>
		/// Дата вылета
		/// </summary>
        public DateOnly? Date { get; set; } = null;

		/// <summary>
		/// Представляет модель критериев поиска в формате строки запроса (Query string)
		/// </summary>
		/// <returns>Модель критериев поиска в формате строки запроса (Query string)</returns>
		public string AsString()
		{
			return $"DepartureAirport={DepartureAirport}&ArrivalAirport={ArrivalAirport}&Date={Date}";
		}
	}
}
