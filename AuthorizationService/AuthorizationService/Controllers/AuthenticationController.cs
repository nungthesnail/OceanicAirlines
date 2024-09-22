using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BuisnessLogic.Models.Authentication;
using BuisnessLogic.Api;
using BuisnessLogic.Api.Exceptions;


namespace AuthenticationService.Controllers
{
    /// <summary>
    /// Контроллер API для авторизации и аутентификации
    /// </summary>
    public class AuthenticationController : Controller
    {
        private BuisnessLogicApi _api;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="api">API бизнес-логики сервиса аутентификации и авторизации</param>
        public AuthenticationController(BuisnessLogicApi api)
        {
            _api = api;
        }

        /// <summary>
        /// Метод API для авторизации. Путь: /api/v1/authenticate. Метод: POST. Разрешен анонимный доступ
        /// </summary>
        /// <param name="request">Запрос авторизации</param>
        /// <returns>Результат авторизации, содержаций JSON Web Token, используемый для авторизации пользователя</returns>
        [Route("/api/v1/authenticate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authorize([FromBody] AuthenticationRequest request)
        {
            AuthenticationResponse result;

            try
            {
                result = await _api.Authenticate(request);
            }
            catch (UserDoesntExistsApiException)
            {
                return BadRequest("User doesn\'t exists");
            }
            catch (UserDoesntHavePasswordApiException)
            {
                return BadRequest("User doesn\'t have a password");
            }
            catch (AuthenticationFailedApiException)
            {
                return BadRequest("Authentication failed");
            }

            return Json(result);
        }
    }
}
