using EntityFrameworkLogic;
using EntityFrameworkLogic.Entities;
using FlightService.Models.Search;
using FlightService.Repository.SearchChainOfResponsibility;

namespace FlightService.Repository
{
    /// <summary>
    /// Организатор поиска запланированного рейса по критериям поиска
    /// </summary>
    public class DatabaseSearcher
    {
        private readonly ApplicationContext _applicationContext;

        private readonly ISearchChainOfResponsibility _searchChain;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="applicationContext">Контекст базы данных</param>
        /// <param name="searchChain">Цепочка ответственности поиска запланированного рейса</param>
        public DatabaseSearcher(ApplicationContext applicationContext, ISearchChainOfResponsibility searchChain)
        {
            _applicationContext = applicationContext;
            _searchChain = searchChain;
        }

        /// <summary>
        /// Инициализирует поиск запланированного рейса по указанным критериям поиска
        /// </summary>
        /// <param name="searchQuery">Критерии поиска запланированного рейса</param>
        /// <returns>Запланированные рейсы, удовлетворяющие критериям поиска</returns>
        public async Task<IEnumerable<SheduledFlight>> Search(FlightSearchModel searchQuery)
        {
            var result = await _searchChain.Handle(searchQuery);

            return result.ToList();
        }
    }
}
