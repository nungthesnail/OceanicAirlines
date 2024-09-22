using FrontendService.Controllers.Exceptions;
using FrontendService.Services.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FrontendService.Controllers
{
	/// <summary>
	/// Контроллер MVC, ответственный за отображение списка бронирований пользователя. Путь: my-bookings
	/// </summary>
	[Route("my-bookings")]
	public class MyBookingsController : Controller
	{
		private readonly IUserBookingsProviderService _userBookingsProvider;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="userBookingsProvider">Сервис, предоставляющий бронирования пользователя</param>
        public MyBookingsController(IUserBookingsProviderService userBookingsProvider)
        {
            _userBookingsProvider = userBookingsProvider;
        }

		/// <summary>
		/// Отображает список бронирований пользователя. Метод: GET. Требуется авторизация.
		/// </summary>
		/// <returns>Список бронирований пользователя</returns>
        [HttpGet]
		[Authorize]
		public async Task<IActionResult> Index()
		{
			var userId = GetUserId();

			var bookings = await _userBookingsProvider.GetUserBookings(userId);

			return View(bookings);
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
	}
}
