using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Controllers
{
	/// <summary>
	/// Контроллер MVC, ответственный за отображение простых информационных страниц
	/// </summary>
	public class PageController : Controller
    {
        /// <summary>
        /// Возвращает страницу с информацией об авиакомпании. Метод: GET. Разрешен анонимный доступ.
        /// </summary>
        /// <returns>Страница с информацией об авиакомпании.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }

		/// <summary>
		/// Возвращает страницу с информацией об контактах авиакомпании. Метод: GET. Разрешен анонимный доступ.
		/// </summary>
		/// <returns>Страница с информацией об контактах авиакомпании.</returns>
		[HttpGet]
        [AllowAnonymous]
        public IActionResult Contacts()
        {
            return View();
        }

        /// <summary>
        /// Показывает страницу об ошибке, код которой передается в теле запроса. Метод: GET. Разрешен анонимный доступ
        /// </summary>
        /// <param name="code">Код ошибки. По умолчанию - 400, Bad Request.</param>
        /// <returns>Страница с информацией об ошибке.</returns>
        [HttpGet]
        [AllowAnonymous]
		public IActionResult Error([FromQuery] int code = 400)
        {
            return View(code);
        }
    }
}
