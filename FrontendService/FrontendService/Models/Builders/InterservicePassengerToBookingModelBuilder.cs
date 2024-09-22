using FrontendService.Models.Booking;
using InterserviceCommunication.Models.BookingService;


namespace FrontendService.Models.Builders
{
	/// <summary>
	/// Строитель модели связки пассажир-к-бронированию для межсервисного взаимодействия
	/// </summary>
	public static class InterservicePassengerToBookingModelBuilder
	{
		/// <summary>
		/// Строит модель связки пассажир-к-бронированию для межсервисного взаимодействия из модели пассажира из формы бронирования
		/// </summary>
		/// <param name="passenger">Модель пассажира из формы бронирования</param>
		/// <returns>Построенная модель связки пассажир-к-бронированию для межсервисного взаимодействия</returns>
		public static BookingServicePassengerToBookingModel BuildFrom(PassengerModel passenger)
		{
			return new BookingServicePassengerToBookingModel()
			{
				Passenger = InterservicePassengerModelBuilder.BuildFrom(passenger),

				MaxBaggageWeight = passenger.MaxBaggageWeight,
				MaxCarryOnWeight = passenger.MaxCarryOnWeight
			};
		}
	}
}
