using FrontendService.ViewModels.Booking;
using FrontendService.ViewModels.Flights;
using InterserviceCommunication.Models.BookingService;

namespace FrontendService.Services.Booking
{
	/// <summary>
	/// Интерфейс строителя модели вида данных о бронировании
	/// </summary>
	public interface IBookingViewModelBuilderService
	{
		/// <summary>
		/// Строит модель вида данных о бронировании
		/// </summary>
		/// <returns>Построенная модель вида данных о бронировании</returns>
		/// <exception cref="InvalidOperationException"></exception>
		public BookingViewModel Build();

		/// <summary>
		/// Строит модель вида данных о бронировании из модели бронирования из межсервисного взаимодействия
		/// </summary>
		/// <param name="bookingModel">Модель бронирования из межсервисного взаимодействия</param>
		/// <returns>Построенная модель вида данных о бронировании</returns>
		/// <exception cref="InvalidOperationException"></exception>
		public Task<BookingViewModel> BuildFrom(BookingServiceBookingModel bookingModel);

		/// <summary>
		/// Устанавливает 10-значный буквенно-численный код бронирования
		/// </summary>
		/// <param name="code">10-значный буквенно-численный код бронирования</param>
		public void SetCode(string code);

		/// <summary>
		/// Устанавливает информацию о запланированном рейсе, на который совершено бронирование
		/// </summary>
		/// <param name="flight">Модель вида запланированного рейса</param>
		public void SetFlight(FlightViewModel flight);

		/// <summary>
		/// Устанавливает информацию о запланированном рейсе, на который совершено бронирование, получая его данные
		/// из микросервиса бронирований
		/// </summary>
		/// <param name="flightId">Идентификатор запланированного рейса</param>
		/// <returns></returns>
		public Task SetFlightFromBookingService(int flightId);

		/// <summary>
		/// Устанавливает список данных пассажиров из бронирования
		/// </summary>
		/// <param name="passengers">Список данных пассажиров из бронирования</param>
		public void SetPassengers(IEnumerable<PassengerViewModel> passengers);

		/// <summary>
		/// Устанавливает список данных пассажиров из бронирования из моделей для межсервисного взаимодействия
		/// </summary>
		/// <param name="serviceModels">Модели связок пассажир-к-бронированию для межсервисного взаимодействия</param>
		public void SetPassengersFromBookingServicesModels(IEnumerable<BookingServicePassengerToBookingModel> serviceModels);
	}
}