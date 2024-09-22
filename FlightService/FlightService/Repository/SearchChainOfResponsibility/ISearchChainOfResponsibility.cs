using EntityFrameworkLogic.Entities;
using FlightService.Models.Search;

namespace FlightService.Repository.SearchChainOfResponsibility
{
    /// <summary>
    /// Цепочка ответственности, обеспечивающая поиск запланированных рейсов путем фильтрации обработчиков полученных
    /// рейсов на соответствие критерию и передачу отфильтрованного множества рейсов далее по цепочке
    /// </summary>
    public interface ISearchChainOfResponsibility
    {
        /// <summary>
        /// Запускает обработку запроса поиска запланированного рейса цепочкой ответственности и возвращает результат
        /// </summary>
        /// <param name="searchQuery">Критерии поиска запланированного рейса</param>
        /// <returns>Запланированные рейсы, удовлетворяющие предоставленным критериям</returns>
        public Task<IQueryable<SheduledFlight>> Handle(FlightSearchModel searchQuery);
    }
}
