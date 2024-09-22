using FrontendService.Models.Flights;
using FrontendService.Services.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FrontendService.Controllers
{
	/// <summary>
	/// Контроллер MVC, ответственный за обработку запроса поиска запланированного рейса.
	/// </summary>
	public class SearchController : Controller
	{
		private readonly ILogger<BookingController> _logger;

		private readonly IFlightSearchService _searchService;

		/// <summary>
		/// Конструктор для внедрения зависимостей.
		/// </summary>
		/// <param name="logger">Логгер</param>
		/// <param name="searchService">Сервис поиска запланированных рейсов</param>
		public SearchController(ILogger<BookingController> logger, IFlightSearchService searchService)
		{
			_logger = logger;
			_searchService = searchService;
		}

		/// <summary>
		/// Отображает список запланированных рейсов, найденных по запросу поиска. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="search">Критерии поиска запланированных рейсов.</param>
		/// <returns>Список запланированных рейсов</returns>
		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Index([FromQuery] FlightSearchModel search)
		{
			var model = await _searchService.Search(search);

			return View(model);
		}
	}
}
