using EntityFrameworkLogic.Entities;
using FlightService.Models.DbBuilders;
using FlightService.Models;
using FlightService.Repository.Exceptions;
using FlightService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SharedFunctionality.Services.Caching;


namespace FlightService.Controllers
{
	/// <summary>
	/// Контроллер API для управления базовыми рейсами в базе данных и кэше
	/// </summary>
	public class BaseFlightsController : Controller
    {
        private readonly DatabaseBaseFlightFacade _dbFacade;

        private readonly ICachingService _cache;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="dbFacade">Фасад базы данных</param>
        /// <param name="cache">Сервис кэширования</param>
        public BaseFlightsController(DatabaseBaseFlightFacade dbFacade, ICachingService cache)
        {
            _dbFacade = dbFacade;
            _cache = cache;
        }

		/// <summary>
		/// Метод API для создания базового рейса в базе данных и кэше. Путь: /api/v1/base-flights. Метод: POST. Требуется авторизация
		/// </summary>
		/// <param name="baseFlight">Модель базового рейса</param>
		/// <returns>Модель, отражающая созданную сущность</returns>
		[Route("/api/v1/base-flights")]
        [HttpPost]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Create([FromBody] BaseFlightModel baseFlight)
        {
            try
            {
                var dbEntity = BaseFlightBuilder.BuildFrom(baseFlight);
                var result = await _dbFacade.Create(dbEntity);

                var response = BaseFlightModel.BuildFrom(result);

                UpdateCache(response);

                return Json(response);
            }
            catch (CreationFailedException)
            {
                return BadRequest("Creation failed. There may be an error in relationed Airports Pair id");
            }
        }

        private void UpdateCache(BaseFlightModel baseFlight)
        {
            _cache.SetWithPrefix("baseflight", baseFlight.Id.ToString(), baseFlight);
            _cache.SetWithPrefix("baseflight-exists", baseFlight.Id.ToString(), new BoolResponseModel(true));
        }

		/// <summary>
		/// Метод API для получения базового рейса из базы данных или кэша. Путь: /api/v1/base-flights/{id}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="id">Идентификатор сущности базового рейса</param>
		/// <returns>Модель, отражающая найденную сущность</returns>
		[Route("/api/v1/base-flights/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            BaseFlightModel? response;

            try
            {
                response = await _cache.GetWithPrefix<string, BaseFlightModel>("baseflight", id.ToString());
            }
            catch (NullReferenceException)
            {
                try
                {
                    var result = _dbFacade.Get(id);

                    response = BaseFlightModel.BuildFrom(result);

                    UpdateCache(response);
                }
                catch (DoesntExistsException)
                {
                    return NotFound();
                }
            }

            return Json(response);
        }

		/// <summary>
		/// Метод API для получения всех базовых рейсов из базы данных или кэша. Путь: /api/v1/base-flights. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <returns>Все базовые рейсы</returns>
		[Route("/api/v1/base-flights")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            EnumerableResponseModel<BaseFlightModel>? response;

            try
            {
                response = await _cache.Get<EnumerableResponseModel<BaseFlightModel>>("baseflights");
            }
            catch (NullReferenceException)
            {
                var result = _dbFacade.GetAll();

                response = CreateEnumerableResponseModel(result);

                await _cache.Set("baseflights", response);
            }

            return Json(response);
        }

        private EnumerableResponseModel<BaseFlightModel> CreateEnumerableResponseModel(IEnumerable<BaseFlight> from)
        {
            var converted = ConvertToAirportModelEnumerable(from);

            var result = EnumerableResponseModel<BaseFlightModel>.Create(converted);

            return result;
        }

        private IEnumerable<BaseFlightModel> ConvertToAirportModelEnumerable(IEnumerable<BaseFlight> baseFlights)
        {
            List<BaseFlightModel> result = [];

            foreach (var item in baseFlights)
            {
                result.Add(BaseFlightModel.BuildFrom(item));
            }

            return result;
        }

		/// <summary>
		/// Метод API для обновления базового рейса в базе данных и кэше. Путь: /api/v1/base-flights. Метод: PUT. Требуется авторизация
		/// </summary>
		/// <param name="baseFlight">Модель базового рейса</param>
		/// <returns>Модель, отражающая обновленную сущность</returns>
		[Route("/api/v1/base-flights")]
        [HttpPut]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Update([FromBody] BaseFlightModel baseFlight)
        {
            try
            {
                var dbEntity = BaseFlightBuilder.BuildFrom(baseFlight);
                var result = await _dbFacade.Update(dbEntity);

                var response = BaseFlightModel.BuildFrom(result);

                UpdateCache(response);

                return Json(response);
            }
            catch (DoesntExistsException)
            {
                return BadRequest("Base flight with specified Id doesn\'t exists");
            }
        }

		/// <summary>
		/// Метод API для удаления базового рейса из базы данных и кэша. Путь: /api/v1/base-flights/{id}. Метод: DELETE. Требуется авторизация
		/// </summary>
		/// <param name="id">Идентификатор сущности базового рейса</param>
		/// <returns>Модель, отражающая удаленную сущность</returns>
		[Route("/api/v1/base-flights/{id:int}")]
        [HttpDelete]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _dbFacade.Delete(id);

                var response = BaseFlightModel.BuildFrom(result);

                DeleteFromCache(response);

                return Json(response);
            }
            catch (DoesntExistsException)
            {
                return BadRequest("Base flight doesn\'t exists");
            }
        }

        private void DeleteFromCache(BaseFlightModel baseFlight)
        {
            _cache.DeleteWithPrefix("baseflight", baseFlight.Id.ToString());
            _cache.SetWithPrefix("baseflight-exists", baseFlight.Id.ToString(), new BoolResponseModel(false));
        }

		/// <summary>
		/// Метод API для проверки существования базового рейса в базе данных.
        /// Путь: /api/v1/base-flights/exists/{id}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="id">Идентификатор сущности базового рейса</param>
		/// <returns>Результат проверки существования базового рейса</returns>
		[Route("/api/v1/base-flights/exists/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Exists(int id)
        {
            BoolResponseModel? response;

            try
            {
                response = await _cache.GetWithPrefix<string, BoolResponseModel>("baseflight-exists", id.ToString());
            }
            catch (NullReferenceException)
            {
                var result = _dbFacade.Exists(id);

                response = BoolResponseModel.Create(result);

                await _cache.SetWithPrefix("baseflight-exists", id.ToString(), response);
            }

            return Json(response);
        }
    }
}
