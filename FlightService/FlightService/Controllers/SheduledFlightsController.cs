using EntityFrameworkLogic.Entities;
using FlightService.Models.DbBuilders;
using FlightService.Models;
using FlightService.Repository.Exceptions;
using FlightService.Repository;
using Microsoft.AspNetCore.Mvc;
using FlightService.Models.Search;
using Microsoft.AspNetCore.Authorization;
using SharedFunctionality.Services.Caching;


namespace FlightService.Controllers
{
	/// <summary>
	/// Контроллер API для управления базовыми рейсами в базе данных и кэше
	/// </summary>
	public class SheduledFlightsController : Controller
    {
        private readonly DatabaseSheduledFlightFacade _dbFacade;

        private DatabaseSearcher _dbSearcher;

        private readonly ICachingService _cache;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="dbFacade">Фасад базы данных</param>
        /// <param name="dbSearcher">Сервис поиска запланированного рейса</param>
        /// <param name="cache">Сервис кэширования</param>
        public SheduledFlightsController(DatabaseSheduledFlightFacade dbFacade, DatabaseSearcher dbSearcher, ICachingService cache)
        {
            _dbFacade = dbFacade;
            _dbSearcher = dbSearcher;
            _cache = cache;
        }

		/// <summary>
		/// Метод API для создания запланированного рейса в базе данных и кэше. Путь: /api/v1/sheduled-flights. Метод: POST. Требуется авторизация
		/// </summary>
		/// <param name="sheduledFlight">Модель запланированного рейса</param>
		/// <returns>Модель, отражающая созданную сущность</returns>
		[Route("/api/v1/sheduled-flights")]
        [HttpPost]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Create([FromBody] SheduledFlightModel sheduledFlight)
        {
            try
            {
                var dbEntity = SheduledFlightBuilder.BuildFrom(sheduledFlight);
                var result = await _dbFacade.Create(dbEntity);

                var response = SheduledFlightModel.BuildFrom(result);

                UpdateCache(response);

                return Json(response);
            }
            catch (CreationFailedException)
            {
                return BadRequest("Creation failed. There may be an error in relationed Base Flight id");
            }
        }

        private void UpdateCache(SheduledFlightModel model)
        {
            _cache.SetWithPrefix("sheduled", model.Id.ToString(), model);
            _cache.SetWithPrefix("sheduled-exists", model.Id.ToString(), new BoolResponseModel(true));
        }

		/// <summary>
		/// Метод API для получения запланированного рейса из базы данных или кэша.
        /// Путь: /api/v1/sheduled-flights/{id}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="id">Идентификатор сущности запланированного рейса</param>
		/// <returns>Модель, отражающая найденную сущность</returns>
		[Route("/api/v1/sheduled-flights/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            SheduledFlightModel? response;

            try
            {
                response = await _cache.GetWithPrefix<string, SheduledFlightModel>("sheduled", id.ToString());
            }
            catch (NullReferenceException)
            {
                try
                {
                    var result = _dbFacade.Get(id);

                    response = SheduledFlightModel.BuildFrom(result);

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
		/// Метод API для получения всех запланированных рейсов из базы данных или кэша.
        /// Путь: /api/v1/sheduled-flights. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <returns>Все запланированные рейсы</returns>
		[Route("/api/v1/sheduled-flights")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
			EnumerableResponseModel<SheduledFlightModel>? response;

            try
            {
                response = await _cache.Get<EnumerableResponseModel<SheduledFlightModel>>("shedules");
            }
            catch (NullReferenceException)
            {
                var result = _dbFacade.GetAll();

                response = CreateEnumerableResponseModel(result);

                await _cache.Set("shedules", response);
            }

            return Json(response);
        }

        private EnumerableResponseModel<SheduledFlightModel> CreateEnumerableResponseModel(IEnumerable<SheduledFlight> from)
        {
            var converted = ConvertToModelEnumerable(from);

            var result = EnumerableResponseModel<SheduledFlightModel>.Create(converted);

            return result;
        }

        private IEnumerable<SheduledFlightModel> ConvertToModelEnumerable(IEnumerable<SheduledFlight> sheduledFlights)
        {
            List<SheduledFlightModel> result = [];

            foreach (var item in sheduledFlights)
            {
                result.Add(SheduledFlightModel.BuildFrom(item));
            }

            return result;
        }

		/// <summary>
		/// Метод API для поиска запланированных рейсов в базе данных или кэше, соответствующих указанным критериям.
		/// Путь: /api/v1/sheduled-flights/search. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="searchQuery">Критерии поиска</param>
		/// <returns>Все рейсы, удовлетворяющие данным критериям поиска</returns>
		[Route("/api/v1/sheduled-flights/search")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromQuery] FlightSearchModel searchQuery)
        {
			EnumerableResponseModel<SheduledFlightModel>? response;

            try
            {
                response = await _cache.GetWithPrefix<string, EnumerableResponseModel<SheduledFlightModel>>("sheduledsearch", searchQuery.AsString());
            }
            catch (NullReferenceException)
            {
                var result = await _dbSearcher.Search(searchQuery);

                response = CreateEnumerableResponseModel(result);

                await _cache.SetWithPrefix("sheduledsearch", searchQuery.AsString(), response);
            }

            return Json(response);
        }

		/// <summary>
		/// Метод API для обновления запланированного рейса в базе данных и кэше. Путь: /api/v1/sheduled-flights. Метод: PUT. Требуется авторизация
		/// </summary>
		/// <param name="sheduledFlight">Модель запланированного рейса</param>
		/// <returns>Модель, отражающая обновленную сущность</returns>
		[Route("/api/v1/sheduled-flights")]
        [HttpPut]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Update([FromBody] SheduledFlightModel sheduledFlight)
        {
            try
            {
                var dbEntity = SheduledFlightBuilder.BuildFrom(sheduledFlight);
                var result = await _dbFacade.Update(dbEntity);

                var response = SheduledFlightModel.BuildFrom(result);

                UpdateCache(response);

                return Json(response);
            }
            catch (DoesntExistsException)
            {
                return BadRequest("Sheduled flight with specified Id doesn\'t exists");
            }
        }

		/// <summary>
		/// Метод API для удаления запланированного рейса из базы данных и кэша. Путь: /api/v1/sheduled-flights/{id}. Метод: DELETE. Требуется авторизация
		/// </summary>
		/// <param name="id">Идентификатор сущности запланированного рейса</param>
		/// <returns>Модель, отражающая удаленную сущность</returns>
		[Route("/api/v1/sheduled-flights/{id:int}")]
        [HttpDelete]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _dbFacade.Delete(id);

                var response = SheduledFlightModel.BuildFrom(result);

                DeleteFromCache(response);

                return Json(response);
            }
            catch (DoesntExistsException)
            {
                return BadRequest("Sheduled flight doesn\'t exists");
            }
        }

        private void DeleteFromCache(SheduledFlightModel model)
        {
            _cache.DeleteWithPrefix("sheduled", model.Id.ToString());
            _cache.SetWithPrefix("sheduled-exists", model.Id.ToString(), new BoolResponseModel(false));
        }

		/// <summary>
		/// Метод API для проверки существования запланированного рейса в базе данных.
		/// Путь: /api/v1/sheduled-flights/exists/{id}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="id">Идентификатор сущности запланированного рейса</param>
		/// <returns>Результат проверки существования запланированного рейса</returns>
		[Route("/api/v1/sheduled-flights/exists/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Exists(int id)
        {
            BoolResponseModel? response;

            try
            {
                response = await _cache.GetWithPrefix<string, BoolResponseModel>("sheduled-exists", id.ToString());
            }
            catch (NullReferenceException)
            {
                var result = _dbFacade.Exists(id);

                response = BoolResponseModel.Create(result);
            }

            return Json(response);
        }
    }
}
