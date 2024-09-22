using BookingService.Models.BookingLogic;
using EntityFrameworkLogic.Entities;

namespace BookingService.Models.DbBuilders
{
    /// <summary>
    /// Строитель сущности бронирования
    /// </summary>
    public static class BookingBuilder
    {
        /// <summary>
        /// Строит сущность бронирования из модели бронирования
        /// </summary>
        /// <param name="model">Модель бронирования</param>
        /// <returns>Построенная сущность бронирования</returns>
        public static Booking BuildFrom(BookingModel model)
        {
            return new Booking
            {
                Id = model.Id,
                Code = model.Code,
                FlightId = model.FlightId,
                CustomerUserId = model.CustomerUserId,
                Confirmed = model.Confirmed,
                CreatedAt = model.CreatedAt
            };
        }
    }
}
