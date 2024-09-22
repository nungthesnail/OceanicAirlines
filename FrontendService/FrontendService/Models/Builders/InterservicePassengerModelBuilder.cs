using FrontendService.Models.Booking;
using InterserviceCommunication.Models.BookingService;

namespace FrontendService.Models.Builders
{
    /// <summary>
    /// Строитель модели пассажира для межсервисного взаимодействия
    /// </summary>
    public static class InterservicePassengerModelBuilder
    {
		/// <summary>
		/// Строит модель пассажира для межсервисного взаимодействия из модели пассажира из формы бронирования
		/// </summary>
		/// <param name="passenger">Модель пассажира из формы бронирования</param>
		/// <returns>Построенная модель пассажира для межсервисного взаимодействия</returns>
		public static BookingServicePassengerModel BuildFrom(PassengerModel passenger)
        {
            return new BookingServicePassengerModel()
            {
                FirstName = passenger.FirstName,
                Surname = passenger.Surname,
                MiddleName = passenger.MiddleName,
                DocumentNumber = passenger.DocumentNumber,
                DocumentIssuerCountry = passenger.DocumentIssuerCountry,
                BirthDate = passenger.BirthDate,
                Gender = passenger.Gender,
                PhoneNumber = passenger.PhoneNumber,
                Email = passenger.Email
            };
        }
    }
}
