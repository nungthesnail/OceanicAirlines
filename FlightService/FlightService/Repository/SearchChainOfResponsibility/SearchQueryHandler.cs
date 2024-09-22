using EntityFrameworkLogic.Entities;
using FlightService.Models.Search;
using Microsoft.EntityFrameworkCore;

namespace FlightService.Repository.SearchChainOfResponsibility
{
    /// <summary>
    /// Базовый класс обработчика запроса поиска запланированного рейса в цепочке ответственности.
    /// Обработчик запроса ответственен только за один критерий поиска
    /// </summary>
    public abstract class SearchQueryHandler : ISearchQueryHandler
    {
        /// <summary>
        /// Следующий обработчик
        /// </summary>
        protected ISearchQueryHandler? _next;

        /// <summary>
        /// Конструктор обработчика поиска. Устанавливает следующий обработчик в цепочке ответственности
        /// </summary>
        /// <param name="next">Следующий</param>
        public SearchQueryHandler(ISearchQueryHandler? next = null)
        {
            _next = next;
        }

        public void SetNext(ISearchQueryHandler next)
        {
            _next = next;
        }

        /// <summary>
        /// Метод, предназначенный для перегрузки, обрабатывающий запрос поиска запланированного рейса.
        /// Обработчик получает от предыдущего обработчика или от инициализатора обработки запроса множество рейсов,
        /// фильтрует его по критерию, за который он ответственен, и передает далее по цепочке обработки или возвращает в
        /// случае отсутствия назначенного следующего обработчика запроса.
        /// Если критерия, за который отвечает обработчик, нет среди критериев поиска, обработчик просто передает запрос дальше по цепочке
        /// или возвращает результат.
        /// </summary>
        /// <param name="searchQuery">Критерии поиска запланированного рейса</param>
        /// <param name="setOfFlights">Множество запланированных рейсов</param>
        /// <returns>Результат обработки запроса</returns>
        public abstract Task<IQueryable<SheduledFlight>> Handle(FlightSearchModel searchQuery, IQueryable<SheduledFlight> setOfFlights);

        protected async Task<IQueryable<SheduledFlight>> ResumeChain(FlightSearchModel searchQuery, IQueryable<SheduledFlight> setOfFlights)
        {
            return await _next!.Handle(searchQuery, setOfFlights);
        }

        protected async Task<bool> HaveToResumeChain(ISearchQueryHandler? nextHandler, IQueryable<SheduledFlight> setOfFlights)
        {
            return nextHandler != null && await setOfFlights.AnyAsync();
        }
    }
}
