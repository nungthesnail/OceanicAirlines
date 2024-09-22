using EntityFrameworkLogic.Entities;
using FlightService.Models.Search;

namespace FlightService.Repository.SearchChainOfResponsibility
{
    /// <summary>
    /// Обработчик запроса поиска запланированного рейса в цепочке ответственности
    /// </summary>
    public interface ISearchQueryHandler
    {
        /// <summary>
        /// Устанавливает следующий обработчик запроса поиска запланированного рейса в цепочке ответственности
        /// </summary>
        /// <param name="next">Следующий обработчик запроса поиска запланированного рейса</param>
        public void SetNext(ISearchQueryHandler next);

        /// <summary>
        /// Обрабатывает запрос поиска запланированного рейса и передает обработку следующему обработчику при необходимости
        /// </summary>
        /// <param name="searchQuery">Критерии поиска запланированного рейса</param>
        /// <param name="setOfFlights">Множество запланированных рейсов</param>
        /// <returns>Запланированные рейсы, удовлетворяющие критериям поиска</returns>
        public Task<IQueryable<SheduledFlight>> Handle(FlightSearchModel searchQuery, IQueryable<SheduledFlight> setOfFlights);
    }
}
