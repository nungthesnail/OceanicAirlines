using FrontendService.ViewModels.Booking;

namespace FrontendService.Services.Booking
{
	/// <summary>
	/// Интерфейс сервиса, предоставляющего бронирования пользователя
	/// </summary>
	public interface IUserBookingsProviderService
	{
		/// <summary>
		/// Получает бронирования пользователя
		/// </summary>
		/// <param name="userId"></param>
		/// <returns>Список бронирований пользователя</returns>
		public Task<IEnumerable<BookingViewModel>> GetUserBookings(Guid userId);
	}
}