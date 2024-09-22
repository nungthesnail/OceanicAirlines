using BookingService.Repository;

namespace BookingService.Services.BookingCodeGenerator
{
    /// <summary>
    /// Интерфейс сервиса генерирования кода бронирования
    /// </summary>
    public interface IBookingCodeGeneratorService
    {
        /// <summary>
        /// Устанавливает фасад контекста базы данных
        /// </summary>
        /// <param name="dbFacade">Фасад контекста базы данных (бронирования)</param>
        public void SetDatabaseBookingFacade(DatabaseBookingFacade dbFacade);

        /// <summary>
        /// Генерирует 10-значный буквенно-численный уникальный код бронирования
        /// </summary>
        /// <returns></returns>
        public string Generate();
    }
}