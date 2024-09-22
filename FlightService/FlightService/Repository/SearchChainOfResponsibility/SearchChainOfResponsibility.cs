using EntityFrameworkLogic;
using EntityFrameworkLogic.Entities;
using FlightService.Models.Search;
using Microsoft.EntityFrameworkCore;


namespace FlightService.Repository.SearchChainOfResponsibility
{
    /// <summary>
    /// Реализация цепочки ответственности поиска запланированного рейса
    /// </summary>
    public class SearchChainOfResponsibility : ISearchChainOfResponsibility
    {
        private ISearchQueryHandler _firstHandler = null!;

        private readonly ApplicationContext _applicationContext;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="applicationContext">Контекст базы данных</param>
        public SearchChainOfResponsibility(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;

            BuildChain();
        }

        public async Task<IQueryable<SheduledFlight>> Handle(FlightSearchModel searchModel)
        {
            var allFlights = GetAllFlights();

            return await _firstHandler.Handle(searchModel, allFlights);
        }

        private IQueryable<SheduledFlight> GetAllFlights()
        {
            return _applicationContext.SheduledFlights
									  .Include(x => x.BaseFlight)
										  .Include(x => x.BaseFlight.AirportsPair)
											  .Include(x => x.BaseFlight.AirportsPair.FirstAirport)
											  .Include(x => x.BaseFlight.AirportsPair.SecondAirport);
        }

        /// <summary>
        /// Метод, строящий цепочку ответственности, включающую в себя все обработчики критериев запроса
        /// </summary>
        private void BuildChain()
        {
            var dateHandler = new SearchDateHandler();
            var departureAirportHandler = new SearchDepartureAirportHandler();
            var arrivalAirportHandler = new SearchArrivalAirportHandler();

            dateHandler.SetNext(departureAirportHandler);
            departureAirportHandler.SetNext(arrivalAirportHandler);

            _firstHandler = dateHandler;
        }
    }
}
