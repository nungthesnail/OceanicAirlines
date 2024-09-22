using InterserviceCommunication.Models.FlightService;


namespace FrontendService.ViewModels.Flights
{
	/// <summary>
	/// Модель вида, описывающая запланированный рейс в списке найденных запланированных рейсов
	/// </summary>
	public class FlightViewModel
	{
		/// <summary>
		/// Идентификатор рейса
		/// </summary>
		public int FlightId { get; set; }

		/// <summary>
		/// Код аэропорта вылета IATA
		/// </summary>
		public string? DepartureAirport { get; set; } = null!;

		/// <summary>
		/// Время вылета
		/// </summary>
		public DateTime SheduledDeparture { get; set; }

		/// <summary>
		/// Код аэропорта прилета IATA
		/// </summary>
		public string? ArrivalAirport { get; set; } = null!;

		/// <summary>
		/// Время приземления
		/// </summary>
		public DateTime SheduledArrival { get; set; }

		/// <summary>
		/// Стоимость бронирования
		/// </summary>
		public decimal Price { get; set; }

		/// <summary>
		/// Количество пассажиров
		/// </summary>
		public int PassengersCount { get; set; }

		/// <summary>
		/// Метод строительства модели вида запланированного рейса из модели запланированного рейса для 
		/// межсервисного взаимодействия. В будущем функционал будет вынесен в отдельный строитель для разделения ответственности классов.
		/// </summary>
		/// <param name="interserviceModel">Модель запланированного рейса для межсервисного взаимодействия</param>
		/// <param name="price">Стоимость бронирования</param>
		/// <param name="passengersCount">Количество пассажиров</param>
		/// <returns></returns>
		public static FlightViewModel BuildFrom
			(FlightServiceSheduledFlightModel interserviceModel, decimal price = 0, int passengersCount = 1)
		{
			return new FlightViewModel
			{
				FlightId = interserviceModel.Id,
				DepartureAirport = interserviceModel.BaseFlight?.AirportsPair?.FirstAirport?.CodeIata,
				SheduledDeparture = interserviceModel.SheduledDeparture,
				ArrivalAirport = interserviceModel.BaseFlight?.AirportsPair?.SecondAirport?.CodeIata,
				SheduledArrival = interserviceModel.SheduledArrival,
				Price = price,
				PassengersCount = passengersCount
			};
		}
	}
}
