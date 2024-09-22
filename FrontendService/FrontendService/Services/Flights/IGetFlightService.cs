using InterserviceCommunication.Models.FlightService;
using FrontendService.Services.Flights.Exceptions;

namespace FrontendService.Services.Flights
{
	/// <summary>
	/// Интерфейс сервиса, предоставляющего информацию о запланированном рейсе
	/// </summary>
	public interface IGetFlightService
	{
		/// <summary>
		/// Предоставляет модель запланированного рейса для межсервисного взаимойдействия
		/// </summary>
		/// <param name="flightId">Идентификатор запланированного рейса</param>
		/// <returns>Модель запланированного рейса для межсервисного взаимодействия</returns>
		/// <exception cref="FlightDoesntExistsException"></exception>
		public Task<FlightServiceSheduledFlightModel> Get(int flightId);
	}
}