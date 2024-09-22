using BookingService.Models.BookingLogic;
using EntityFrameworkLogic.Entities;

namespace BookingService.Models.DbBuilders
{
	/// <summary>
	/// Строитель сущности связки пассажир-к-бронированию
	/// </summary>
	public static class PassengerToBookingBuilder
    {
		/// <summary>
		/// Строит сущность связки пассажир-к-бронированию из модели связки
		/// </summary>
		/// <param name="model">Модель связки пассажир-к-бронированию</param>
		/// <returns>Построенная сущность связки пассажир-к-бронированию</returns>
		public static PassengerToBooking BuildFrom(PassengerBookingModel model)
        {
            return new PassengerToBooking
            {
                Id = model.Id,
                CarryOnMaxWeight = model.CarryOnMaxWeight,
                BaggageMaxWeight = model.BaggageMaxWeight
            };
        }
    }
}
