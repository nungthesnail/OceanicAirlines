namespace FrontendService.ViewModels.Flights
{
	/// <summary>
	/// Модель вида, описывающая результаты поиска запланированных рейсов
	/// </summary>
	public class SearchViewModel
	{
		/// <summary>
		/// Критерии поиска запланированных рейсов
		/// </summary>
		public FlightSearchViewModel SearchQuery { get; set; } = null!;

		/// <summary>
		/// Найденные запланированные рейсы
		/// </summary>
		public IEnumerable<FlightViewModel> Flights { get; set; } = null!;
	}
}
