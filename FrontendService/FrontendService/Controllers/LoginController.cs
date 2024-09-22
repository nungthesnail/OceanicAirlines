using FrontendService.Models.Authorization;
using FrontendService.Services.Login;
using FrontendService.Services.Login.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FrontendService.Controllers
{
	/// <summary>
	/// Контроллер MVC, ответственный за авторизацию, выход из аккаунта и отображение страницы входа на сайт
	/// </summary>
	public class LoginController : Controller
	{
		/// <summary>
		/// Название куки, в которую помещается JSON Web Token, устанавливающий личность и данные пользователя
		/// </summary>
		private const string _tokenCookieName = "token";

		private readonly ILoginService _loginService = null!;

		/// <summary>
		/// Контроллер для внедрения зависимостей
		/// </summary>
		/// <param name="loginService">Сервис авторизации</param>
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

		/// <summary>
		/// Отображает страницу с формой авторизации. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <returns>Страница с формой авторизации</returns>
		[HttpGet]
		[AllowAnonymous]
        public IActionResult Index()
		{
			return View(true);
		}

		/// <summary>
		/// Производит выход из аккаунта. Метод: GET. Требуется авторизация
		/// </summary>
		/// <returns>Перенаправление на домашнуюю страницу</returns>
		[HttpGet]
		[Authorize]
		public IActionResult Logout()
		{
			DeleteTokenCookie();

			return Redirect("/");
		}

		private void DeleteTokenCookie()
		{
			Response.Cookies.Delete(_tokenCookieName);
		}

		/// <summary>
		/// Производит авторизацию. При ошибке авторизации показывается форма бронирования с подписью об ошибке.
		/// В будущем функционал будет выполнен посредством AJAX-запросов. Метод: POST. Разрешен анонимный доступ
		/// </summary>
		/// <param name="loginModel">Данные, необходимые для авторизации</param>
		/// <returns>Перенаправление на главную страницу или сообщение об ошибке авторизации</returns>
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromForm] LoginModel loginModel)
		{
			try
			{
				var token = await _loginService.Login(loginModel);

				SetTokenCookie(token);
			}
			catch (LoginFailedException)
			{
				return View("Index", false);
			}

			return Redirect("/");
		}

		private void SetTokenCookie(string token)
		{
			Response.Cookies.Append(_tokenCookieName, token);
		}
	}
}
