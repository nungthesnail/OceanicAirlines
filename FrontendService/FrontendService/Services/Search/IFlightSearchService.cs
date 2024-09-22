using FrontendService.Models.Flights;
using FrontendService.ViewModels.Flights;

namespace FrontendService.Services.Search
{
	/// <summary>
	/// Сервис, осуществляющий поиск запланированных рейсов по заданным критериям
	/// </summary>
	public interface IFlightSearchService
	{
		/// <summary>
		/// Ищет запланированные рейсы по заданным критериям
		/// </summary>
		/// <param name="search">Критерии поиска запланированного рейса</param>
		/// <returns>Модель вида, отражающая результаты поиска запланированных рейсов</returns>
		public Task<SearchViewModel> Search(FlightSearchModel search);
	}
}
