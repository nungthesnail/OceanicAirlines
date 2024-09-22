using InterserviceCommunication.Connectors;
using InterserviceCommunication.Models;
using InterserviceCommunication.Models.FlightService;
using InterserviceCommunication.Exceptions;

namespace InterserviceCommunication.Requests.FlightService
{
	/// <summary>
	/// Запрос поиска запланированных рейсов
	/// </summary>
	public class FlightServiceSearchFlightRequest : InterserviceRequest
	{
		private FlightServiceConnector _connector;

		private FlightSearchQueryModel _searchQuery;

		/// <summary>
		/// Конструктор запроса
		/// </summary>
		/// <param name="connector">Коннектор</param>
		/// <param name="searchQuery">Критерии поиска запланированного рейса</param>
		public FlightServiceSearchFlightRequest(FlightServiceConnector connector, FlightSearchQueryModel searchQuery)
		{
			_httpMethod = HttpMethod.Get;
			_route = "api/v1/sheduled-flights/search";

			_connector = connector;
			_searchQuery = searchQuery;
		}

		public override Connector GetConnector() => _connector;

		public override string BuildRoute()
		{
			var queryString = _searchQuery.BuildQueryString();

			var result = queryString.Any() ? BuildRouteWithQuery(queryString) : _route;

			return result;
		}

		private string BuildRouteWithQuery(string queryString)
		{
			return $"{_route}/?{queryString}";
		}

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Полученные запланированные рейсы</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<EnumerableResponseModel<FlightServiceSheduledFlightModel>> Send()
		{
			return await _connector.Send(this);
		}
	}
}
