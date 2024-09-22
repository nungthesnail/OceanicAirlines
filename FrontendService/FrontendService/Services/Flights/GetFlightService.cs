using InterserviceCommunication;
using InterserviceCommunication.Exceptions;
using InterserviceCommunication.Models.FlightService;
using FrontendService.Services.Flights.Exceptions;


namespace FrontendService.Services.Flights
{
	/// <summary>
	/// Сервис, предоставляющий информацию о запланированном рейсе
	/// </summary>
	public class GetFlightService : IGetFlightService
	{
		private readonly InterserviceCommunicator _interserviceCommunicator;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="interserviceCommunicator">Межсервисный связист</param>
		public GetFlightService(InterserviceCommunicator interserviceCommunicator)
		{
			_interserviceCommunicator = interserviceCommunicator;
		}

		public async Task<FlightServiceSheduledFlightModel> Get(int flightId)
		{
			try
			{
				var connector = _interserviceCommunicator.GetFlightServiceConnector();
				var request = connector.CreateGetFlightRequest(flightId);
				var flight = await request.Send();

				return flight;
			}
			catch (NotFoundException)
			{
				throw new FlightDoesntExistsException();
			}
		}
	}
}
