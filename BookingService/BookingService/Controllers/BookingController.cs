using BookingService.BookingLogic;
using BookingService.BookingLogic.Exceptions;
using BookingService.Models.BookingLogic;
using BookingService.Repository;
using BookingService.Repository.Exceptions;
using BookingService.Services.NotificationSender;
using EntityFrameworkLogic.Entities;
using InterserviceCommunication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedFunctionality.Services.Caching;
using System.ComponentModel.DataAnnotations;


namespace BookingService.Controllers
{
    /// <summary>
    /// Контроллер API для управления бронированиями
    /// </summary>
    public class BookingController : Controller
    {
        private readonly BookingManagerService _bookingManager;

        private readonly DatabaseBookingFacade _dbBooking;

        private readonly ICachingService _cache;

        private readonly INotificationSenderService _notifier;

        private readonly NotificationBuilder _notificationBuilder;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="bookingManager">Сервис оформления бронирования </param>
        /// <param name="dbBooking">Фасад контекста базы данных (бронирования)</param>
        /// <param name="cache">Сервис кеширования</param>
        /// <param name="notifier">Сервис уведомлений</param>
        /// <param name="notificationBuilder">Строитель уведомлений</param>
		public BookingController(
            BookingManagerService bookingManager,
            DatabaseBookingFacade dbBooking,
            ICachingService cache,
            INotificationSenderService notifier,
            NotificationBuilder notificationBuilder)
        {
            _bookingManager = bookingManager;
            _dbBooking = dbBooking;
            _cache = cache;
            _notifier = notifier;
            _notificationBuilder = notificationBuilder;
        }

		/// <summary>
		/// Оформляет бронирование и отправляет уведомление о нем пользователю. Добавляет бронирование в базу данных и кэш.
        /// Путь: /api/v1/booking. Метод: POST. Требуется авторизация
		/// </summary>
		/// <param name="booking">Модель бронирования</param>
		/// <returns>Модель, отражающая созданное бронирование</returns>
		[Route("/api/v1/booking")]
        [HttpPost]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Book([FromBody] BookingModel booking)
        {
            try
            {
                var result = await _bookingManager.Book(booking);

                UpdateCache(result);

                SendNotification(result);

                return Json(result);
            }
            catch (InvalidCustomerUserException exc)
            {
                return BadRequest($"InvalidCustomerUser {exc.Message}");
            }
            catch (InvalidFlightException exc)
            {
                return BadRequest($"InvalidFlight {exc.Message}");
            }
            catch (NotEnoughSeatsException exc)
            {
                return BadRequest($"NotEnoughSeats {exc.Message}");
            }
            catch (PassengerDuplicationException exc)
            {
                return BadRequest($"DuplicatedPassenger {exc.Message}");
            }
            catch (WrongNumberOfEntitiesException exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"CreationError {exc.Message}");
            }
            catch (WrongPassengerDataException exc)
            {
                return BadRequest($"WrongPassengerData {exc.Message}");
            }
            catch (BookingRegistrationException exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"BookingRegistrationError {exc.Message}");
            }
            catch (ValidationException exc)
            {
                return BadRequest($"DataValidationFailed {exc.Message}");
            }
        }

		private void UpdateCache(BookingModel booking)
        {
            _cache.SetWithPrefix("booking", booking.Id.ToString(), booking);
        }

        private void SendNotification(BookingModel booking)
        {
            var notification = CreateNotification(booking);

            _notifier.Send(notification);
        }

        private Notification CreateNotification(BookingModel booking)
        {
			_notificationBuilder.SetReceiverType(Notification.ReceiversTypes.RECEIVER_USERID);
			_notificationBuilder.SetReceiverData(booking.CustomerUserId.ToString());
			_notificationBuilder.SetMessage(booking);

			return _notificationBuilder.Build();
		}

		/// <summary>
		/// Метод API, получающий бронирование из кэша или базы данных. Путь: /api/v1/booking/{id}. Метод: GET. Требуется авторизация
		/// </summary>
		/// <param name="id">Идентификатор бронирования</param>
		/// <returns>Найденное бронирование</returns>
		[Route("/api/v1/booking/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Get(int id)
        {
            BookingModel? response;

            try
            {
                response = await _cache.GetWithPrefix<string, BookingModel>("booking", id.ToString());
            }
            catch (NullReferenceException)
            {
                try
                {
                    var result = _dbBooking.Get(id);

                    response = BookingModel.BuildFrom(result);

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
		/// Метод API, возвращающий все бронирования пользователя. Путь: /api/v1/user-bookings/{userId}. Метод: GET. Требуется авторизация
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Все бронирования пользователя</returns>
		[Route("/api/v1/user-bookings/{userId:guid}")]
		[HttpGet]
		[Authorize(Roles = "Admin,Microservice")]
		public IActionResult GetByCustomerUserId(Guid userId)
		{
			try
			{
				var result = _dbBooking.GetByCustomerUserId(userId);

				var converted = ConvertDbBookingToModel(result);

                var response = CreateEnumerableResponseModel(converted);

				return Json(response);
			}
			catch (DoesntExistsException)
			{
				return NotFound();
			}
		}

        private IEnumerable<BookingModel> ConvertDbBookingToModel(IEnumerable<Booking> bookings)
        {
            return from b in bookings
				   select BookingModel.BuildFrom(b);
		}

        private EnumerableResponseModel<BookingModel> CreateEnumerableResponseModel(IEnumerable<BookingModel> bookings)
        {
            return new EnumerableResponseModel<BookingModel>
            {
                Result = bookings
            };
        }

		/// <summary>
		/// Метод API, удаляющий бронирование. Путь: /api/v1/booking/{id}. Метод: DELETE. Требуется авторизация
		/// </summary>
		/// <param name="id">Идентификатор бронирования</param>
		/// <returns>Удаленное бронирование</returns>
		[Route("/api/v1/booking/{id:int}")]
        [HttpDelete]
        [Authorize(Roles = "Admin,Microservice")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _dbBooking.Delete(id);
                var response = BookingModel.BuildFrom(result);

                DeleteFromCache(response);

                return Json(response);
            }
            catch (DoesntExistsException)
            {
                return NotFound();
            }
        }

        private void DeleteFromCache(BookingModel booking)
        {
            _cache.DeleteWithPrefix("booking", booking.Id.ToString());
        }
    }
}
