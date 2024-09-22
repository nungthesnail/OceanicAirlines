using BookingService.Models.BookingLogic;
using EntityFrameworkLogic.Entities;

namespace BookingService.Models.DbBuilders
{
	/// <summary>
	/// Строитель сущности пассажира
	/// </summary>
	public static class PassengerBuilder
    {
		/// <summary>
		/// Строит сущность пассажира из модели пассажира
		/// </summary>
		/// <param name="model">Модель пассажира</param>
		/// <returns>Построенная сущность пассажира</returns>
		public static Passenger BuildFrom(PassengerModel model)
        {
            return new Passenger
            {
                Id = model.Id,
                FirstName = model.FirstName,
                Surname = model.Surname,
                MiddleName = model.MiddleName,
                DocumentNumber = model.DocumentNumber,
                DocumentIssuerCountry = model.DocumentIssuerCountry,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email
            };
        }
    }
}
