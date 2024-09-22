using AuthenticationService.Models;
using BuisnessLogic.Api;
using BuisnessLogic.Api.Exceptions;
using BuisnessLogic.Models.Management;
using BuisnessLogic.Repository.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedFunctionality.Services.Caching;


namespace AuthenticationService.Controllers
{
    /// <summary>
    /// Контроллер API для управления хешами паролей пользователей в базе данных
    /// </summary>
    public class ManagementController : Controller
    {
        private readonly BuisnessLogicApi _api;

        private readonly ICachingService _cache;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="api">API бизнес-логики сервиса аутентификации и авторизации</param>
        /// <param name="cache">Сервис кэширования</param>
        public ManagementController(BuisnessLogicApi api, ICachingService cache)
        {
            _api = api;
            _cache = cache;
        }

        /// <summary>
        /// Метод API для получения хеша пароля пользователя из кэша или базы данных. Путь: /api/v1/hash/userId. Метод: GET. Требуется авторизация
        /// </summary>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <returns>Модель хеша пароля пользователя</returns>
        [Route("/api/v1/hash/{userId:guid}")]
        [HttpGet]
        [Authorize(Roles = "Admin,User,Microservice")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var clientUserId = GetClientUserId();

            var hasTechnicalRole = CheckHasTechicalRole();

            ManagementResponse? result;

            if (UserHasRights(userId, clientUserId, hasTechnicalRole))
            {
                try
                {
                    result = await _cache.GetWithPrefix<string, ManagementResponse>("pwdhash", userId.ToString());

                    await _cache.SetWithPrefix("pwdhash", userId.ToString(), result);
                }
                catch (NullReferenceException)
                {
                    try
                    {
                        result = _api.GetByUser(userId);
                    }
                    catch (UserDoesntExistsApiException)
                    {
                        return BadRequest();
                    }
                    catch (UserDoesntHavePasswordApiException)
                    {
                        return BadRequest();
                    }
                }
            }
            else
            {
                return Forbid();
            }

            return Json(result);
        }

        private string GetClientUserId()
        {
            return User.Claims
                       .Where(x => x.Type == "UserId")
                       .FirstOrDefault()!
                       .Value;
        }

        private bool CheckHasTechicalRole()
        {
            return User.IsInRole("Admin") || User.IsInRole("Microservice");
        }

        private bool UserHasRights(Guid requestingUserId, string clientUserId, bool hasTechnicalRole)
        {
            return clientUserId == requestingUserId.ToString() || hasTechnicalRole;
        }

		/// <summary>
		/// Метод API для создания хеша пароля пользователя в кэше и базе данных. Путь: /api/v1/hash. Метод: POST. Разрешен анонимный доступ
		/// </summary>
		/// <param name="request">Модель пароля пользователя</param>
		/// <returns>Созданный хеш пароля пользователя</returns>
		[Route("/api/v1/hash")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] ManagementRequest request)
        {
            try
            {
                var result = await _api.Create(request);

                UpdateCache(result);

                return Json(result);
            }
            catch (UserDoesntExistsApiException)
            {
                return BadRequest("User doesn\'t exists");
            }
            catch (UserAlreadyLinkedException)
            {
                return BadRequest("User already has password");
            }
        }

        private void UpdateCache(ManagementResponse model)
        {
            _cache.SetWithPrefix("pwdhash", model.LinkedUserId.ToString(), model);
            _cache.SetWithPrefix("haspwd", model.LinkedUserId.ToString(), new BoolResponseModel(true));
        }

		/// <summary>
		/// Метод API для обновления хеша пароля пользователя в кэше и базе данных. Путь: /api/v1/hash. Метод: PUT. Требуется авторизация
		/// </summary>
		/// <param name="request">Модель пароля пользователя</param>
		/// <returns>Обновленный хеш пароля пользователя</returns>
		[Route("/api/v1/hash")]
        [HttpPut]
        [Authorize(Roles = "User,Microservice")]
        public async Task<IActionResult> Update([FromBody] ManagementRequest request)
        {
            var clientUserId = GetClientUserId();

            var hasTechnicalRole = CheckHasTechicalRole();

            ManagementResponse result;

            if (UserHasRights(request.LinkedUserId, clientUserId, hasTechnicalRole))
            {
                try
                {
                    result = await _api.Update(request);

                    UpdateCache(result);
                }
                catch (BadRequestApiException)
                {
                    return BadRequest();
                }
            }
            else
            {
                return Forbid();
            }

            return Json(result);
        }

		/// <summary>
		/// Метод API для удаления хеша пароля пользователя из кэша и базы данных. Путь: /api/v1/hash/userId. Метод: DELETE. Требуется авторизация
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Модель удаленного хеша пароля пользователя</returns>
		[Route("/api/v1/hash/{userId:guid}")]
        [HttpDelete]
        [Authorize(Roles = "User,Microservice")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            var clientUserId = GetClientUserId();

            var hasTechnicalRole = CheckHasTechicalRole();

            ManagementResponse result;

            if (UserHasRights(userId, clientUserId, hasTechnicalRole))
            {
                try
                {
                    result = await _api.Delete(userId);

                    DeleteFromCache(result);
                }
                catch (UserDoesntHavePasswordApiException)
                {
                    return BadRequest();
                }
            }
            else
            {
                return Forbid();
            }

            return Json(result);
        }

        private void DeleteFromCache(ManagementResponse model)
        {
            _cache.DeleteWithPrefix("pwdhash", model.LinkedUserId.ToString());
            _cache.SetWithPrefix("haspwd", model.LinkedUserId.ToString(), new BoolResponseModel(false));
        }

		/// <summary>
		/// Метод API для проверки существования пароля у пользователя. Путь: /api/v1/hash/userId. Метод: GET. Требуется авторизация
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Существование пароля у пользователя</returns>
		[Route("/api/v1/check-user-has-password/{userId:guid}")]
        [HttpGet]
        [Authorize(Roles = "Admin,User,Microservice")]
        public async Task<IActionResult> CheckUserHasPassword(Guid userId)
        {
            BoolResponseModel? result;

            try
            {
                result = await _cache.GetWithPrefix<string, BoolResponseModel>("haspwd", userId.ToString());
            }
            catch (NullReferenceException)
            {
                var hasPassword = _api.UserHasPassword(userId);

                result = MakeBoolResponse(hasPassword);

                await _cache.SetWithPrefix("haspwd", userId.ToString(), result);
            }

            return Json(result);
        }

        private BoolResponseModel MakeBoolResponse(bool value)
        {
            return new BoolResponseModel()
            {
                Result = value
            };
        }
    }
}
