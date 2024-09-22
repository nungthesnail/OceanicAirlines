using FrontendService.Models.Flights;

namespace FrontendService.ViewModels.Flights
{
	/// <summary>
	/// Модель вида, описывающая критерии поиска запланированных рейсов
	/// </summary>
	public class FlightSearchViewModel
	{
		/// <summary>
		/// Код аэропорта вылета IATA
		/// </summary>
		public string DepartureAirport { get; set; } = null!;

		/// <summary>
		/// Код аэропорта прилета IATA
		/// </summary>
		public string ArrivalAirport { get; set; } = null!;

		/// <summary>
		/// Дата вылета
		/// </summary>
		public DateOnly DepartureDate { get; set; }

		/// <summary>
		/// Количество пассажиров
		/// </summary>
		public int PassengersCount { get; set; }

		/// <summary>
		/// Метод построения модели вида критериев поиска запланированных рейсов из модели критериев поиска.
		/// В будущем функционал будет вынесен в отдельный строитель для разделения ответственности классов.
		/// </summary>
		/// <param name="searchModel">Критерии поиска запланированного рейса</param>
		/// <returns>Построенная модель вида критериев поиска запланированных рейсов</returns>
		public static FlightSearchViewModel BuildFrom(FlightSearchModel searchModel)
		{
			return new()
			{
				DepartureAirport = searchModel.DepartureAirport,
				ArrivalAirport = searchModel.ArrivalAirport,
				DepartureDate = searchModel.DepartureDate,
				PassengersCount = searchModel.PassengerCount
			};
		}
	}
}
