using InterserviceCommunication.Connectors;
using InterserviceCommunication.Models.FlightService;
using InterserviceCommunication.Exceptions;


namespace InterserviceCommunication.Requests.FlightService
{
    /// <summary>
    /// Запрос получения запланированного рейса
    /// </summary>
    public class FlightServiceGetFlightRequest : InterserviceRequest
    {
        private FlightServiceConnector _connector;

        private int _flightId;

        /// <summary>
        /// Конструктор запроса
        /// </summary>
        /// <param name="connector">Коннектор</param>
        /// <param name="flightId">Идентификатор запланированного рейса</param>
        public FlightServiceGetFlightRequest(FlightServiceConnector connector, int flightId)
        {
            _httpMethod = HttpMethod.Get;
            _route = "api/v1/sheduled-flights";

            _connector = connector;
            _flightId = flightId;
        }

        public override Connector GetConnector() => _connector;

        public override string BuildRoute()
        {
            return $"{_route}/{_flightId}";
        }

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Полученный запланированный рейс</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<FlightServiceSheduledFlightModel> Send()
        {
            return await _connector.Send(this);
        }
    }
}
