using System.Web;

namespace InterserviceCommunication.Models.FlightService
{
	/// <summary>
	/// Модель критериев поиска запланированного рейса
	/// </summary>
	public class FlightSearchQueryModel
	{
		/// <summary>
		/// Код аэропорта вылета IATA
		/// </summary>
		public string? DepartureAirport = null;

		/// <summary>
		/// Код аэропорта прилета IATA
		/// </summary>
		public string? ArrivalAirport = null;

		/// <summary>
		/// Дата вылета
		/// </summary>
		public DateOnly? Date = null;

		/// <summary>
		/// Строит тело запроса поиска из критериев поиска запланированного рейса
		/// </summary>
		/// <returns>Тело запроса поиска запланированного рейса</returns>
		public string BuildQueryString()
		{
			var queryBuilder = HttpUtility.ParseQueryString(String.Empty);

			if (DepartureAirport != null)
				queryBuilder.Add("DepartureAirport", DepartureAirport);
			if (ArrivalAirport != null)
				queryBuilder.Add("ArrivalAirport", ArrivalAirport);
			if (Date != null)
				queryBuilder.Add("Date", Date.ToString());

			var result = queryBuilder.ToString();

			return result ?? "";
		}
	}
}
