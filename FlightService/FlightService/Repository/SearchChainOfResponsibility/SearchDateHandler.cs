using EntityFrameworkLogic.Entities;
using FlightService.Models.Search;


namespace FlightService.Repository.SearchChainOfResponsibility
{
	/// <summary>
	/// Обработчик запроса поиска запланированного рейса. Отвечает в цепочке ответственности за фильтрацию запланированных
	/// рейсов, вылетающий в указанную дату
	/// </summary>
	public class SearchDateHandler : SearchQueryHandler
    {
        /// <summary>
        /// Конструктор обработчика запроса. Устанавливает следующий обработчик запроса
        /// </summary>
        /// <param name="next">Следующий обработчик запроса</param>
        public SearchDateHandler(ISearchQueryHandler? next = null)
            : base(next)
        { }

		/// <summary>
		/// Обрабатывает запрос поиска запланированного рейса. Фильтрует входное множество рейсов на соответствие критерию
		/// вылета в указанную дату
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
            return searchQuery.Date != null;
        }

        private IQueryable<SheduledFlight> SelectSatysfyingFlights(FlightSearchModel searchQuery, IQueryable<SheduledFlight> flights)
        {
            return flights.Where(x => DateOnly.FromDateTime(x.SheduledDeparture).Equals(searchQuery.Date));
        }
    }
}
