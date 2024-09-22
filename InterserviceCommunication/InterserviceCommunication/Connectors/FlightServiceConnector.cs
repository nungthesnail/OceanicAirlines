using InterserviceCommunication.Models;
using InterserviceCommunication.Models.FlightService;
using InterserviceCommunication.Requests.FlightService;
using InterserviceCommunication.Exceptions;


namespace InterserviceCommunication.Connectors
{
	/// <summary>
	/// Коннектор микросервиса рейсов
	/// </summary>
	public class FlightServiceConnector : Connector
    {
        private FlightServiceConnector()
        { }

		/// <summary>
		/// Конструктор коннектора микросервиса пользователей
		/// </summary>
		/// <param name="communicator">Межсервисный связист</param>
		/// <param name="settings">Настройки коннектора</param>
		public FlightServiceConnector(InterserviceCommunicator communicator, ConnectorSettings settings)
        {
            _communicator = communicator;
            _settings = settings;
        }

		/// <summary>
		/// Отправляет запрос получения запланированного рейса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Полученный запланированный рейс</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<FlightServiceSheduledFlightModel> Send(FlightServiceGetFlightRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();

            var result = await Send(method, route);

            var responseModel = await DeserializeHttpContent<FlightServiceSheduledFlightModel>(result.Content);

            return responseModel!;
        }

		/// <summary>
		/// Отправляет запрос поиска запланированного рейса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Полученные запланированные рейсы</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<EnumerableResponseModel<FlightServiceSheduledFlightModel>> Send
            (FlightServiceSearchFlightRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();

            var result = await Send(method, route);

            var responseModel = 
                await DeserializeHttpContent<EnumerableResponseModel<FlightServiceSheduledFlightModel>>(result.Content);

            return responseModel!;
        }

		/// <summary>
		/// Создает запрос получения запланированного рейса
		/// </summary>
		/// <param name="flightId">Идентификатор запланированного рейса</param>
		/// <returns>Запрос получения запланированного рейса</returns>
		public FlightServiceGetFlightRequest CreateGetFlightRequest(int flightId)
        {
            return new FlightServiceGetFlightRequest(this, flightId);
        }

		/// <summary>
		/// Создает запрос поиска запланированного рейса
		/// </summary>
		/// <param name="searchQuery">Критерии поиска запланированного рейса</param>
		/// <returns>Запрос поиска запланированного рейса</returns>
		public FlightServiceSearchFlightRequest CreateSearchFlightRequest(FlightSearchQueryModel searchQuery)
        {
			return new FlightServiceSearchFlightRequest(this, searchQuery);
		}

		/// <summary>
		/// Создает запрос поиска запланированного рейса
		/// </summary>
		/// <param name="departureAirport">Код аэропорта вылета IATA</param>
		/// <param name="arrivalAirport">Код аэропорта прилета IATA</param>
		/// <param name="date">Дата вылета</param>
		/// <returns>Запрос поиска запланированного рейса</returns>
		public FlightServiceSearchFlightRequest CreateSearchFlightRequest
            (string? departureAirport = null, string? arrivalAirport = null, DateOnly? date = null)
        {
            var searchQuery = BuildSearchQueryModel(
                departureAirport: departureAirport,
                arrivalAirport: arrivalAirport,
                date: date);

            return new FlightServiceSearchFlightRequest(this, searchQuery);
        }

        private FlightSearchQueryModel BuildSearchQueryModel
            (string? departureAirport = null, string? arrivalAirport = null, DateOnly? date = null)
        {
            return new FlightSearchQueryModel
            {
                DepartureAirport = departureAirport,
                ArrivalAirport = arrivalAirport,
                Date = date
            };
        }

	}
}
