using FrontendService.Models.Booking;
using FrontendService.Models.Builders;
using FrontendService.Controllers.Exceptions;
using InterserviceCommunication;
using InterserviceCommunication.Exceptions;
using InterserviceCommunication.Models.BookingService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace FrontendService.Controllers
{
	/// <summary>
	/// Контроллер MVC, ответственный за обработку запроса бронирования, вывод формы бронирования и отображения ошибки бронирования
	/// </summary>
	public class BookingController : Controller
	{
		private readonly ILogger<BookingController> _logger;

		private readonly InterserviceCommunicator _interserviceCommunicator;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="logger">Логгер</param>
		/// <param name="interserviceCommunicator">Межсервисный связист</param>
		public BookingController(
			ILogger<BookingController> logger,
			InterserviceCommunicator interserviceCommunicator)
        {
            _logger = logger;
			_interserviceCommunicator = interserviceCommunicator;
        }

		/// <summary>
		/// Возвращает страницу с формой бронирования. Метод: POST. Требуется авторизация
		/// </summary>
		/// <param name="bookingData">Данные, необходимые для начала бронирования</param>
		/// <returns>Страница с формой бронирования</returns>
        [HttpPost]
		[Authorize]
		public IActionResult StartBooking([FromForm] BookingInitialDataModel bookingData)
		{
			return View(bookingData);
		}

		/// <summary>
		/// Возвращает страницой с информацией об ошибке бронирования. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <returns>Страница с информацией об ошибке бронирования</returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult BookingError()
		{
			return View();
		}

		/// <summary>
		/// Обрабатывает запрос бронирования, посылая запрос микросервису бронирований
		/// и возвращая ошибку при неудавшемся бронировании или отсутствии нужных данных о пользователе,
		/// совершающим бронирование. Путь: /booking/book/{flightId}. Метод: POST. Требуется авторизация
		/// </summary>
		/// <param name="flightId">Идентификатор запланированного рейса, на который совершается бронирование</param>
		/// <param name="passengers">Список пассажиров из формы бронирования</param>
		/// <returns></returns>
		[HttpPost]
		[Route("[controller]/[action]/{flightId:int}")]
		[Authorize]
		public async Task<IActionResult> Book(int flightId, [FromForm] PassengerModel[] passengers)
		{
			try
			{
				var bookingModel = CreateInterserviceBookingModel(flightId, passengers);

				var bookedResult = await InvokeBookingServiceToBook(bookingModel);
			}
			catch (RequiredIdentityClaimIsntSpecifiedException)
			{
				return Unauthorized();
			}
			catch (InterserviceCommunicationException)
			{
				return RedirectToAction("BookingError");
			}

			return Redirect("/");
		}

		private BookingServiceBookingModel CreateInterserviceBookingModel(int flightId, PassengerModel[] passengers)
		{
			var userId = GetUserId();

			var result = InterserviceBookingModelBuilder.Build(flightId, userId, passengers);

			return result;
		}

		private Guid GetUserId()
		{
			var userIdClaim = User.Claims.Where(x => x.Type == "UserId").FirstOrDefault()?.Value;

			ThrowIfNull(userIdClaim);

			return Guid.Parse(userIdClaim!);
		}

		private void ThrowIfNull(string? value)
		{
			if (value == null)
			{
				throw new RequiredIdentityClaimIsntSpecifiedException();
			}
		}

		private async Task<BookingServiceBookingModel> InvokeBookingServiceToBook(BookingServiceBookingModel bookingModel)
		{
			var connector = _interserviceCommunicator.GetBookingServiceConnector();
			var request = connector.CreateBookRequest(bookingModel);
			var response = await request.Send();

			return response;
		}
	}
}
