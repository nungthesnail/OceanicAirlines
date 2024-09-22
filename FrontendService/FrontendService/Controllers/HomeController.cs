using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FrontendService.Controllers
{
	/// <summary>
	/// Контроллер MVC, ответственный за отображение начальной страницы веб-приложения с формой поиска запланированных рейсов
	/// </summary>
	public class HomeController : Controller
    {
		/// <summary>
		/// Отображает начальную страницу веб-приложения с формой поиска запланированных рейсов
		/// </summary>
		/// <returns>Начальная страницв веб-приложения с формой поиска запланированных рейсов</returns>
		[HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
