using FlightService.Repository;
using FlightService.Models;
using Microsoft.AspNetCore.Mvc;
using FlightService.Models.DbBuilders;
using FlightService.Repository.Exceptions;
using EntityFrameworkLogic.Entities;
using Microsoft.AspNetCore.Authorization;
using SharedFunctionality.Services.Caching;


namespace FlightService.Controllers
{
    /// <summary>
    /// Контроллер API для управления аэропортами в базе данных и кэше
    /// </summary>
    public class AirportsController : Controller
    {
        private readonly DatabaseAirportFacade _dbFacade;

        private readonly ICachingService _cache;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="dbFacade">Фасад базы данных</param>
        /// <param name="cache">Сервис кэширования</param>
        public AirportsController(DatabaseAirportFacade dbFacade, ICachingService cache)
        {
            _dbFacade = dbFacade;
            _cache = cache;
        }

		/// <summary>
		/// Метод API для создания аэропорта в базе данных и кэше. Путь: /api/v1/airports. Метод: POST. Требуется авторизация
		/// </summary>
		/// <param name="airport">Модель аэропорта</param>
		/// <returns>Модель, отражающая созданную сущность аэропорта</returns>
		[Route("/api/v1/airports")]
        [HttpPost]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Create([FromBody] AirportModel airport)
        {
            try
            {
                var dbEntity = AirportBuilder.BuildFrom(airport);
                var result = await _dbFacade.Create(dbEntity);

                var response = AirportModel.BuildFrom(result);

                UpdateCache(response);

                return Json(response);
            }
            catch (AlreadyExistsException)
            {
                return BadRequest("Airport already exists");
            }
            catch (CreationFailedException)
            {
                return BadRequest("Creation failed. There may be an error in IATA Code format");
            }
        }

        private void UpdateCache(AirportModel airport)
        {
            _cache.SetWithPrefix("airport", airport.Id.ToString(), airport);
			_cache.SetWithPrefix("airport", airport.CodeIata, airport);

			_cache.SetWithPrefix("airport-exists", airport.Id.ToString(), new BoolResponseModel(true));
			_cache.SetWithPrefix("airport-exists", airport.CodeIata, new BoolResponseModel(true));
		}

		/// <summary>
		/// Метод API для получения аэропорта из базы данных или кэша. Путь: /api/v1/airports/{id}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="id">Идентификатор сущности аэропорта</param>
		/// <returns>Модель, отражающая найденную сущность аэропорта</returns>
		[Route("/api/v1/airports/{id:int}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
			AirportModel? response;

			try
			{
				response = await _cache.GetWithPrefix<string, AirportModel>("airport", id.ToString());
			}
			catch (NullReferenceException)
			{
				try
				{
					var result = _dbFacade.Get(id);

					response = AirportModel.BuildFrom(result);

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
		/// Метод API для получения аэропорта из базы данных или кэша. Путь: /api/v1/airports/{iataCode}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="iataCode">Код аэропорта IATA</param>
		/// <returns>Модель, отражающая найденную сущность аэропорта</returns>
		[Route("/api/v1/airports/{iataCode:alpha:length(3)}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(string iataCode)
        {
            AirportModel? response;

            try
            {
                response = await _cache.GetWithPrefix<string, AirportModel>("airport", iataCode);
            }
            catch (NullReferenceException)
            {
                try
                {
                    var result = _dbFacade.GetByIataCode(iataCode);

                    response = AirportModel.BuildFrom(result);

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
		/// Метод API для получения всех аэропортов из базы данных или кэша. Путь: /api/v1/airports. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <returns>Все аэропорты</returns>
		[Route("/api/v1/airports")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            EnumerableResponseModel<AirportModel>? response;

            try
            {
                response = await _cache.Get<EnumerableResponseModel<AirportModel>>("airports");
            }
            catch (NullReferenceException)
            {
                var result = _dbFacade.GetAll();

                response = CreateEnumerableResponseModel(result);

                await _cache.Set("airports", response);
            }

            return Json(response);
        }

        private EnumerableResponseModel<AirportModel> CreateEnumerableResponseModel(IEnumerable<Airport> from)
        {
            var converted = ConvertToAirportModelEnumerable(from);

            var result = EnumerableResponseModel<AirportModel>.Create(converted);

            return result;
        }

        private IEnumerable<AirportModel> ConvertToAirportModelEnumerable(IEnumerable<Airport> airports)
        {
            List<AirportModel> result = [];

            foreach (var item in airports)
            {
                result.Add(AirportModel.BuildFrom(item));
            }

            return result;
        }

		/// <summary>
		/// Метод API для обновления аэропорта в базе данных и кэше. Путь: /api/v1/airports. Метод: PUT. Требуется авторизация
		/// </summary>
		/// <param name="airport">Модель аэропорта</param>
		/// <returns>Модель, отражающая обновленную сущность аэропорта</returns>
		[Route("/api/v1/airports")]
        [HttpPut]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Update([FromBody] AirportModel airport)
        {
            try
            {
                var dbEntity = AirportBuilder.BuildFrom(airport);
                var result = await _dbFacade.Update(dbEntity);

                var response = AirportModel.BuildFrom(result);

                UpdateCache(response);

                return Json(response);
            }
            catch (DoesntExistsException)
            {
                return BadRequest("Airport with specified Id doesn\'t exists");
            }
        }

		/// <summary>
		/// Метод API для удаления аэропорта из базы данных и кэша. Путь: /api/v1/airports/{id}. Метод: DELETE. Требуется авторизация
		/// </summary>
		/// <param name="id">Идентификатор сущности аэропорта</param>
		/// <returns>Модель, отражающая удаленную сущность аэропорта</returns>
		[Route("/api/v1/airports/{id:int}")]
        [HttpDelete]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _dbFacade.Delete(id);

                var response = AirportModel.BuildFrom(result);

                DeleteFromCache(response);

                return Json(response);
            }
            catch (DoesntExistsException)
            {
                return BadRequest("Airport doesn\'t exists");
            }
        }

        private void DeleteFromCache(AirportModel airport)
        {
            _cache.DeleteWithPrefix("airport", airport.Id.ToString());
            _cache.DeleteWithPrefix("airport", airport.CodeIata);

            _cache.SetWithPrefix("airport-exists", airport.Id.ToString(), new BoolResponseModel(false));
			_cache.SetWithPrefix("airport-exists", airport.CodeIata, new BoolResponseModel(false));
		}

		/// <summary>
		/// Метод API для проверки существования аэропорта в базе данных. Путь: /api/v1/airports/exists/{id}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="id">Идентификатор сущности аэропорта</param>
		/// <returns>Результат проверки существования аэропорта</returns>
		[Route("/api/v1/airports/exists/{id:int}")]
        [HttpGet]
        public async Task<IActionResult> Exists(int id)
        {
            BoolResponseModel? response;

            try
            {
                response = await _cache.GetWithPrefix<string, BoolResponseModel>("airport-exists", id.ToString());
            }
            catch (NullReferenceException)
            {
                var result = _dbFacade.Exists(id);

                response = BoolResponseModel.Create(result);

                await _cache.SetWithPrefix("airport-exists", id.ToString(), response);
            }

            return Json(response);
        }

		/// <summary>
		/// Метод API для проверки существования аэропорта в базе данных. Путь: /api/v1/airports/exists/{iataCode}. Метод: GET. Разрешен анонимный доступ
		/// </summary>
		/// <param name="iataCode">Код аэропорта IATA</param>
		/// <returns>Результат проверки существования аэропорта</returns>
		[Route("/api/v1/airports/exists/{iataCode:alpha:length(3)}")]
        [HttpGet]
        public async Task<IActionResult> Exists(string iataCode)
        {
			BoolResponseModel? response;

			try
			{
				response = await _cache.GetWithPrefix<string, BoolResponseModel>("airport-exists", iataCode);
			}
			catch (NullReferenceException)
			{
				var result = _dbFacade.Exists(iataCode);

				response = BoolResponseModel.Create(result);

				await _cache.SetWithPrefix("airport-exists", iataCode, response);
			}

			return Json(response);
		}
    }
}
