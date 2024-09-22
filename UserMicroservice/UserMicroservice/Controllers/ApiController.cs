using BuisnessLogic;
using BuisnessLogic.Api;
using BuisnessLogic.Models;
using Microsoft.AspNetCore.Mvc;
using BuisnessLogic.Api.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using SharedFunctionality.Utils.Accounts;
using SharedFunctionality.Utils.Accounts.Exceptions;
using SharedFunctionality.Utils.Email;
using SharedFunctionality.Utils.Email.Exceptions;
using SharedFunctionality.Services.Caching;


namespace UserMicroservice.Controllers
{
    /// <summary>
    /// Контроллер API сервиса пользователей
    /// </summary>
    public class ApiController : Controller
	{
        private readonly ICachingService _cache;

		private readonly ILogger<ApiController> _logger;

        private readonly IConfiguration _config;

		private BuisnessLogicApi _api = null!;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="cache">Сервис кэширования</param>
        /// <param name="logger">Логгер</param>
        /// <param name="config">Конфигурация приложения</param>
        public ApiController(ICachingService cache, ILogger<ApiController> logger, IConfiguration config)
		{
			_cache = cache;
			_logger = logger;
			_config = config;

			BuildBuisnessLogicApi();
        }

        private void BuildBuisnessLogicApi()
        {
			var builder = new BuisnessLogicApiBuilder();
            builder.SetConfiguration(_config);

			_api = builder.Build();
		}

        /// <summary>
        /// API метод создания пользователя в кэше и базе данных. Путь: /api/v1/user. Метод: POST. Разрешен анонимный доступ
        /// </summary>
        /// <param name="request">Модель пользователя</param>
        /// <returns>Созданный пользователь</returns>
        [Route("/api/v1/user")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] RequestUserModel request)
        {
            try
            {
                SetRequestRoleToUser(ref request);
                CheckRequestFieldsAreValid(request);

                var result = await _api.Create(request);

                UpdateCache(result);

                return Json(result);
            }
            catch (AccountNameIsntValidException)
            {
                return BadRequest("User name has wrong format");
            }
            catch (EmailAddressIsntValidException)
            {
                return BadRequest("Email address has wrong format");
            }
            catch (UserAlreadyExistsApiException)
            {
                return BadRequest("User already exists");
            }
        }

        private void SetRequestRoleToUser(ref RequestUserModel request)
        {
            request.Role = "User";
        }

        private void CheckRequestFieldsAreValid(RequestUserModel request)
        {
            ThrowIfUserNameIsntValid(request.Name);
            ThrowIfEmailIsntValid(request.Email);
        }

        private void ThrowIfUserNameIsntValid(string userName)
        {
            if (!AccountUtils.IsValidName(userName))
            {
                throw new AccountNameIsntValidException();
            }
        }

        private void ThrowIfEmailIsntValid(string email)
        {
            if (!EmailUtils.IsValidAddress(email))
            {
                throw new EmailAddressIsntValidException();
            }
        }

        private void UpdateCache(ResponseUserModel user)
        {
            _cache.Set(user.Id.ToString(), user);
            _cache.SetWithPrefix("username", user.Name, user);

			_cache.SetWithPrefix("userexists", user.Id.ToString(), new BoolResponseModel(true));
			_cache.SetWithPrefix("userexists", user.Name, new BoolResponseModel(true));
		}

		/// <summary>
		/// API метод получения модели пользователя из кэша или базы данных. Путь: /api/v1/user/{userId}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Найденный пользователь</returns>
		[Route("/api/v1/user/{userId:guid}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid userId)
        {
            ResponseUserModel? result;

            try
            {
                result = await _cache.Get<ResponseUserModel>(userId.ToString());
            }
            catch (NullReferenceException)
            {
                try
                {
                    result = _api.Get(userId);

                    UpdateCache(result);
                }
                catch (UserDoesntExistsApiException)
                {
					return NotFound("User doesn\'t exists");
				}
            }

            return CreateResponse(result!);
		}

        private IActionResult CreateResponse(ResponseUserModel model)
        {
            // HideEmailFieldIfRequired(ref model);

            return Json(model);
        }

        private void HideEmailFieldIfRequired(ref ResponseUserModel result)
        {
            var isTechnical = UserHaveTechnicalRole();
            var userGetsHimself = GetUserId(User.Claims) == result.Id;

            if (!isTechnical && !userGetsHimself)
            {
                result.Email = String.Empty;
            }
        }

		/// <summary>
		/// API метод получения модели пользователя из кэша или базы данных. Путь: /api/v1/user/{userName}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Найденный пользователь</returns>
		[Route("/api/v1/user/{userName:regex(^[[a-zA-Z_$]][[\\w$]]*$)}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string userName)
        {
			ResponseUserModel? result;

			try
			{
				result = await _cache.GetWithPrefix<string, ResponseUserModel>("username", userName);
			}
			catch (NullReferenceException)
			{
				try
				{
					result = _api.Get(userName);

                    UpdateCache(result);
				}
				catch (UserDoesntExistsApiException)
				{
					return NotFound("User doesn\'t exists");
				}
			}

			return CreateResponse(result!);
		}

		/// <summary>
		/// API метод обновления пользователя в кэше и базе данных. Путь: /api/v1/user. Метод: PUT. Требуется авторизация
		/// </summary>
		/// <param name="request">Модель пользователя</param>
		/// <returns>Обновленный пользователь</returns>
		[Route("/api/v1/user")]
        [HttpPut]
        [Authorize(Roles = "Admin,User,Microservice")]
        public async Task<IActionResult> Update([FromBody] RequestUserModel request)
        {
            try
            {
                if (UserHasRights(request.Id))
                {
                    ForbidChangeRole(ref request);

                    var result = await _api.Update(request);

                    UpdateCache(result);

					return Json(result);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (UserDoesntExistsApiException)
            {
                return BadRequest("User doesn\'t exists");
            }
        }

        private bool UserHasRights(Guid userId)
        {
            if (UserHaveTechnicalRole())
            {
                return true;
            }
            else if (GetUserId(User.Claims) == userId)
            {
                return true;
            }

            return false;
        }

        private bool UserHaveTechnicalRole()
        {
            var a = User.IsInRole("Microservice");
            var b = User.IsInRole("Admin");

			return User.IsInRole("Microservice") || User.IsInRole("Admin");
        }

        private bool UserHasRights(string userName)
        {
            if (User.IsInRole("Microservice") || User.IsInRole("Admin"))
            {
                return true;
            }
            else if (GetUserName(User.Claims) == userName)
            {
                return true;
            }

            return false;
        }

        private Guid GetUserId(IEnumerable<Claim> claims)
        {
            try
            {
                var idClaim = User.Claims
                                  .Where(x => x.Type == "UserId")
                                  .FirstOrDefault();

                var result = Guid.Parse(idClaim?.Value ?? "");

                return result;
            }
            catch (FormatException)
            {
                return Guid.Empty;
            }
        }

        private string GetUserName(IEnumerable<Claim> claims)
        {
            var nameClaim = User.Claims
                                .Where(x => x.Type == "UserName")
                                .FirstOrDefault();

            return nameClaim?.Value ?? "";
        }

        private void ForbidChangeRole(ref RequestUserModel request)
        {
            var roleClaim = User.Claims
                                .Where(x => x.Type == ClaimTypes.Role)
                                .FirstOrDefault();

            request.Role = roleClaim?.Value; // Здесь баг
        }

		/// <summary>
		/// API метод удаления пользователя из кэша и базы данных. Путь: /api/v1/user/{userId}. Метод: DELETE. Требуется авторизация
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Удаленный пользователь</returns>
		[Route("/api/v1/user/{userId:guid}")]
        [HttpDelete]
        [Authorize(Roles = "Admin,User,Microservice")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            try
            {
                if (UserHasRights(userId))
                {
                    var result = await _api.Delete(userId);

                    DeleteFromCache(result);

					return Json(result);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (UserDoesntExistsApiException)
            {
                return BadRequest("User doesn\'t exists");
            }
        }

		/// <summary>
		/// API метод удаления пользователя из кэша и базы данных. Путь: /api/v1/user/{userId}. Метод: DELETE. Требуется авторизация
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Удаленный пользователь</returns>
		[Route("/api/v1/user/{userName:regex(^[[a-zA-Z_$]][[\\w$]]*$)}")]
        [HttpDelete]
        [Authorize(Roles = "Admin,User,Microservice")]
        public async Task<IActionResult> Delete(string userName)
        {
            try
            {
                if (UserHasRights(userName))
                {
                    var result = await _api.Delete(userName);

                    DeleteFromCache(result);

					return Json(result);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (UserDoesntExistsApiException)
            {
                return BadRequest("User doesn\'t exists");
            }
		}

		private void DeleteFromCache(ResponseUserModel user)
		{
			_cache.DeleteWithPrefix("username", user.Name);
			_cache.Delete(user.Id.ToString());

			_cache.SetWithPrefix("userexists", user.Name, new BoolResponseModel(false));
			_cache.SetWithPrefix("userexists", user.Id.ToString(), new BoolResponseModel(false));
		}

		/// <summary>
		/// API метод для проверки существования пользователя. Путь: /api/v1/user-exists/{userId}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Существование пользователя</returns>
		[Route("/api/v1/user-exists/{userId:guid}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserExists(Guid userId)
        {
            BoolResponseModel? result;

            try
            {
                result = await _cache.GetWithPrefix<string, BoolResponseModel>("userexists", userId.ToString());
            }
            catch (NullReferenceException)
            {
                result = _api.UserExists(userId);

				await _cache.SetWithPrefix("userexists", userId.ToString(), result);
			}

            return Json(result);
        }

		/// <summary>
		/// API метод для проверки существования пользователя. Путь: /api/v1/user-exists/{userName}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Существование пользователя</returns>
		[Route("/api/v1/user-exists/{userName:regex(^[[a-zA-Z_$]][[\\w$]]*$)}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> UserExists(string userName)
        {
			BoolResponseModel? result;

			try
			{
				result = await _cache.GetWithPrefix<string, BoolResponseModel>("userexists", userName);
			}
			catch (NullReferenceException)
			{
				result = _api.UserExists(userName);

                await _cache.SetWithPrefix("userexists", userName, result);
			}

			return Json(result);
		}
    }
}