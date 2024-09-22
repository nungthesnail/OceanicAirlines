using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FrontendService.Controllers
{
	/// <summary>
	/// Контроллер MVC, ответственный за отображение страницы пользователя, включающей в себя
	/// информацию о пользователе и доступные действия с пользователем
	/// </summary>
	public class MyAccountController : Controller
	{
		/// <summary>
		/// Отображает страницу пользователя, включающей в себя информацию о пользователе и доступные действия с пользователем.
		/// Метод: GET. Требуется авторизация
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize]
		public IActionResult Index()
		{
			return View();
		}
	}
}
