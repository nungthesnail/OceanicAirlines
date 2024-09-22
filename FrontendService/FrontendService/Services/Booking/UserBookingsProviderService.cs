using FrontendService.ViewModels.Booking;
using InterserviceCommunication;
using InterserviceCommunication.Models.BookingService;


namespace FrontendService.Services.Booking
{
	/// <summary>
	/// Сервис, предоставляющий бронирования пользователя
	/// </summary>
	public class UserBookingsProviderService : IUserBookingsProviderService
	{
		private readonly InterserviceCommunicator _interserviceCommunicator;

		private readonly IBookingViewModelBuilderService _bookingBuilder;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="interserviceCommunicator">Межсервисный связист</param>
		/// <param name="bookingBuilder">Строитель моделей вида данных о бронировании</param>
		public UserBookingsProviderService(
			InterserviceCommunicator interserviceCommunicator,
			IBookingViewModelBuilderService bookingBuilder)
		{
			_interserviceCommunicator = interserviceCommunicator;
			_bookingBuilder = bookingBuilder;
		}

		public async Task<IEnumerable<BookingViewModel>> GetUserBookings(Guid userId)
		{
			var connector = _interserviceCommunicator.GetBookingServiceConnector();
			var request = connector.CreateGetUserBookingsRequest(userId);
			var result = await request.Send();

			var response = await ConvertToViewModel(result);

			return response;
		}

		private async Task<IEnumerable<BookingViewModel>> ConvertToViewModel(IEnumerable<BookingServiceBookingModel> bookings)
		{
			List<BookingViewModel> result = [];

			foreach (var booking in bookings)
			{
				result.Add(await _bookingBuilder.BuildFrom(booking));
			}

			return result;
		}
	}
}
