using EntityFrameworkLogic.Entities;
using FlightService.Models.Search;


namespace FlightService.Repository.SearchChainOfResponsibility
{
    /// <summary>
    /// Обработчик запроса поиска запланированного рейса. Отвечает в цепочке ответственности за фильтрацию запланированных
    /// рейсов, прилетающих в указанный аэропорт
    /// </summary>
    public class SearchArrivalAirportHandler : SearchQueryHandler
    {
        /// <summary>
        /// Конструктор обработчика запроса, устанавливающий следующий обработчик в цепочке ответственности
        /// </summary>
        /// <param name="next">Следующий обработчик запроса</param>
        public SearchArrivalAirportHandler(ISearchQueryHandler? next = null)
            : base(next)
        { }

		/// <summary>
		/// Обрабатывает запрос поиска запланированного рейса. Фильтрует входное множество рейсов на соответствие критерию
		/// аэропорта прилета
		/// </summary>
		/// <param name="searchQuery">Критерии поиска запланированного рейса</param>
		/// <param name="setOfFlights">Множество рейсов</param>
		/// <returns>Рейсы, удовлетворяющие критериям поиска</returns>
		public override async Task<IQueryable<SheduledFlight>> Handle(FlightSearchModel searchQuery, IQueryable<SheduledFlight> setOfFlights)
        {
            var result = setOfFlights;

            if (IsResponsible(searchQuery))
            {
                result = SelectSatysfyingFlights(searchQuery, result);
            }

            if (await HaveToResumeChain(_next, result))
            {
                result = await ResumeChain(searchQuery, result);
            }

            return result;
        }

        private bool IsResponsible(FlightSearchModel searchQuery)
        {
            return searchQuery.ArrivalAirport != null;
        }

        private IQueryable<SheduledFlight> SelectSatysfyingFlights(FlightSearchModel searchQuery, IQueryable<SheduledFlight> flights)
        {
            return flights.Where(x =>
                                 x.BaseFlight.AirportsPair.SecondAirport.CodeIata
                                 == searchQuery.ArrivalAirport);
        }
    }
}
