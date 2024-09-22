using FrontendService.Models.Authorization;
using FrontendService.Services.Register;
using FrontendService.Services.Register.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FrontendService.Controllers
{
	/// <summary>
	/// Контроллер MVC, ответственный за обработку запроса регистрации пользователя, отображение формы регистрации пользователя
	/// и отображение ошибок регистрации
	/// </summary>
	public class RegisterController : Controller
	{
		private readonly IRegisterProviderService _registerProvider;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="registerProvider">Сервис регистрации пользователя</param>
        public RegisterController(IRegisterProviderService registerProvider)
        {
            _registerProvider = registerProvider;
        }

		/// <summary>
		/// Возвращает страницу с формой регистрации пользователя. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <returns>Страница с формой регистрации пользователя.</returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Index()
		{
			return View(true);
		}

		/// <summary>
		/// Обрабатывает запрос регистрации пользователя и показывает информацию об ошибке регистрации 
		/// в случае возникновения таковой. В будущем функционал будет выполнен посредством AJAX-запросов. Метод: POST. Разрешен анонимный доступ
		/// </summary>
		/// <param name="model"></param>
		/// <returns>Перенаправление на страницу авторизации или отображение ошибки о регистрации</returns>
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register([FromForm] RegisterModel model)
		{
			try
			{
				await _registerProvider.Register(model);

				return Redirect("/login");
			}
			catch (RegisterFailedException)
			{
				return View("Index", false);
			}
		}
	}
}
