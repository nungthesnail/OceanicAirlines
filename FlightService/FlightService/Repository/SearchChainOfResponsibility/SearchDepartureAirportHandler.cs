using EntityFrameworkLogic.Entities;
using FlightService.Models.Search;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;


namespace FlightService.Repository.SearchChainOfResponsibility
{
	/// <summary>
	/// Обработчик запроса поиска запланированного рейса. Отвечает в цепочке ответственности за фильтрацию запланированных
	/// рейсов, вылетающих из указанного аэропорта
	/// </summary>
	public class SearchDepartureAirportHandler : SearchQueryHandler
    {
		/// <summary>
		/// Конструктор обработчика запроса, устанавливающий следующий обработчик в цепочке ответственности
		/// </summary>
		/// <param name="next">Следующий обработчик запроса</param>
		public SearchDepartureAirportHandler(ISearchQueryHandler? next = null)
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
            return searchQuery.DepartureAirport != null;
        }

        private IQueryable<SheduledFlight> SelectSatysfyingFlights(FlightSearchModel searchQuery, IQueryable<SheduledFlight> flights)
        {
            return flights.Where(x => 
                                 x.BaseFlight.AirportsPair.FirstAirport.CodeIata.ToUpper()
                                 == searchQuery.DepartureAirport!.ToUpper());
        }
    }
}
