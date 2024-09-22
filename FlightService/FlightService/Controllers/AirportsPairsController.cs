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
    /// Контроллер API для управления парами аэропортов в базе данных и кэше
    /// </summary>
    public class AirportsPairsController : Controller
    {
        private readonly DatabaseAirportsPairFacade _dbFacade;

        private readonly ICachingService _cache;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="dbFacade">Фасад базы данных</param>
        /// <param name="cache">Сервис кэширования</param>
        public AirportsPairsController(DatabaseAirportsPairFacade dbFacade, ICachingService cache)
        {
            _dbFacade = dbFacade;
            _cache = cache;
        }

		/// <summary>
		/// Метод API для создания пары аэропортов в базе данных и кэше. Путь: /api/v1/airports-pairs. Метод: POST. Требуется авторизация
		/// </summary>
		/// <param name="airportsPair">Модель пары аэропортов</param>
		/// <returns>Модель, отражающая созданную сущность</returns>
		[Route("/api/v1/airports-pairs")]
        [HttpPost]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Create([FromBody] AirportsPairModel airportsPair)
        {
            try
            {
                var dbEntity = AirportsPairBuilder.BuildFrom(airportsPair);
                var result = await _dbFacade.Create(dbEntity);

                var response = AirportsPairModel.BuildFrom(result);

                UpdateCache(response);

                return Json(response);
            }
            catch (CreationFailedException)
            {
                return BadRequest("Creation failed. There may be an error in relationed airports id\'s");
            }
        }

		private void UpdateCache(AirportsPairModel pair)
		{
			_cache.SetWithPrefix("airportspair", pair.Id.ToString(), pair);
			_cache.SetWithPrefix("airportspair-exists", pair.Id.ToString(), new BoolResponseModel(true));
		}

		/// <summary>
		/// Метод API для получения пары аэропортов из базы данных или кэша. Путь: /api/v1/airports-pairs/{id}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="id">Идентификатор сущности пары аэропортов</param>
		/// <returns>Модель, отражающая найденную сущность</returns>
		[Route("/api/v1/airports-pairs/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            AirportsPairModel? response;

            try
            {
                response = await _cache.GetWithPrefix<string, AirportsPairModel>("airportspair", id.ToString());
            }
            catch (NullReferenceException)
            {
                try
                {
                    var result = _dbFacade.Get(id);

                    response = AirportsPairModel.BuildFrom(result);

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
		/// Метод API для получения всех пар аэропортов из базы данных или кэша. Путь: /api/v1/airports-pairs. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <returns>Все пары аэропортов</returns>
		[Route("/api/v1/airports-pairs")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            EnumerableResponseModel<AirportsPairModel>? response;

            try
            {
                response = await _cache.Get<EnumerableResponseModel<AirportsPairModel>>("airportspairs");
            }
            catch (NullReferenceException)
            {
                var result = _dbFacade.GetAll();

                response = CreateEnumerableResponseModel(result);

                await _cache.Set("airportspairs", response);
            }

            return Json(response);
        }

        private EnumerableResponseModel<AirportsPairModel> CreateEnumerableResponseModel(IEnumerable<AirportsPair> from)
        {
            var converted = ConvertToAirportModelEnumerable(from);

            var result = EnumerableResponseModel<AirportsPairModel>.Create(converted);

            return result;
        }

        private IEnumerable<AirportsPairModel> ConvertToAirportModelEnumerable(IEnumerable<AirportsPair> airportsPairs)
        {
            List<AirportsPairModel> result = [];

            foreach (var item in airportsPairs)
            {
                result.Add(AirportsPairModel.BuildFrom(item));
            }

            return result;
        }

		/// <summary>
		/// Метод API для обновления пары аэропортов в базе данных и кэше. Путь: /api/v1/airports-pairs. Метод: PUT. Требуется авторизация
		/// </summary>
		/// <param name="airportsPair">Модель пары аэропортов</param>
		/// <returns>Модель, отражающая обновленную сущность аэропортов в базе данных</returns>
		[Route("/api/v1/airports-pairs")]
        [HttpPut]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Update([FromBody] AirportsPairModel airportsPair)
        {
            try
            {
                var dbEntity = AirportsPairBuilder.BuildFrom(airportsPair);
                var result = await _dbFacade.Update(dbEntity);

                var response = AirportsPairModel.BuildFrom(result);

                UpdateCache(response);

                return Json(response);
            }
            catch (DoesntExistsException)
            {
                return BadRequest("Airports pair with specified Id doesn\'t exists");
            }
        }

		/// <summary>
		/// Метод API для удаления пар аэропортов из базы данных или кэша. Путь: /api/v1/airports-pairs/{id}. Метод: DELETE. Требуется авторизация
		/// </summary>
		/// <param name="id">Идентификатор сущности аэропорта</param>
		/// <returns></returns>
		[Route("/api/v1/airports-pairs/{id:int}")]
        [HttpDelete]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _dbFacade.Delete(id);

                var response = AirportsPairModel.BuildFrom(result);

                DeleteFromCache(response);

                return Json(response);
            }
            catch (DoesntExistsException)
            {
                return BadRequest("Airports pair doesn\'t exists");
            }
        }

        private void DeleteFromCache(AirportsPairModel pair)
        {
            _cache.DeleteWithPrefix("airportspair", pair.Id.ToString());
            _cache.SetWithPrefix("airportspair-exists", pair.Id.ToString(), new BoolResponseModel(false));
        }

		/// <summary>
		/// Метод API для проверки существования пары аэропортов в базе данных.
        /// Путь: /api/v1/airports-pairs/exists/{id}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="id">Идентификатор сущности пары аэропортов</param>
		/// <returns>Результат проверки существования пары аэропортов</returns>
		[Route("/api/v1/airports-pairs/exists/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Exists(int id)
        {
            BoolResponseModel? response;

            try
            {
                response = await _cache.GetWithPrefix<string, BoolResponseModel>("airportspair-exists", id.ToString());
            }
            catch (NullReferenceException)
            {
                var result = _dbFacade.Exists(id);

                response = BoolResponseModel.Create(result);

                await _cache.SetWithPrefix("airportspair-exists", id.ToString(), response);
            }

            return Json(response);
        }
    }
}
