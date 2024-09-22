using FrontendService.Models.Flights;
using FrontendService.ViewModels.Flights;
using InterserviceCommunication.Models.FlightService;
using InterserviceCommunication;


namespace FrontendService.Services.Search
{
	public class FlightSearchService : IFlightSearchService
	{
		/// <summary>
		/// Сервис поиска запланированных рейсов по заданным критериям
		/// </summary>
		private readonly InterserviceCommunicator _interserviceCommunicator;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="interserviceCommunicator">Межсервисный связист</param>
        public FlightSearchService(InterserviceCommunicator interserviceCommunicator)
        {
			_interserviceCommunicator = interserviceCommunicator;
        }

        public async Task<SearchViewModel> Search(FlightSearchModel search)
		{
			var searchQuery = FlightSearchViewModel.BuildFrom(search);

			var flights = await SearchFlights(search, 0, search.PassengerCount);

			var model = BuildSearchViewModel(searchQuery, flights);

			return model;
		}

		private async Task<IEnumerable<FlightViewModel>> SearchFlights
			(FlightSearchModel search, decimal price = 0, int passengersCount = 1)
		{
			var interserviceSearchQuery = BuildSearchQuery(search);
			var response = await MakeSearchRequestToFlightService(interserviceSearchQuery);

			var flights = ConvertToViewModels(response, price, passengersCount);

			return flights;
		}

		private FlightSearchQueryModel BuildSearchQuery(FlightSearchModel search)
		{
			return new FlightSearchQueryModel
			{
				DepartureAirport = search.DepartureAirport,
				ArrivalAirport = search.ArrivalAirport,
				Date = search.DepartureDate
			};
		}

		private async Task<IEnumerable<FlightServiceSheduledFlightModel>> MakeSearchRequestToFlightService(FlightSearchQueryModel searchQuery)
		{
			var connector = _interserviceCommunicator.GetFlightServiceConnector();
			var request = connector.CreateSearchFlightRequest(searchQuery);
			var response = await request.Send();
			var flights = response.Result;
			return flights;
		}

		private IEnumerable<FlightViewModel> ConvertToViewModels
			(IEnumerable<FlightServiceSheduledFlightModel> source, decimal price = 0, int passengersCount = 1)
		{
			var converted = from f in source
							select FlightViewModel.BuildFrom(f, price, passengersCount);

			return converted;
		}

		private SearchViewModel BuildSearchViewModel(FlightSearchViewModel search, IEnumerable<FlightViewModel> flights)
		{
			return new SearchViewModel
			{
				SearchQuery = search,
				Flights = flights
			};
		}
	}
}
