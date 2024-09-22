using InterserviceCommunication.Connectors;
using InterserviceCommunication.Models.BookingService;
using InterserviceCommunication.Exceptions;


namespace InterserviceCommunication.Requests.BookingService
{
	/// <summary>
	/// Запрос получения бронирований пользователя
	/// </summary>
	public class BookingServiceGetUserBookingsRequest : InterserviceRequest
	{
		private BookingServiceConnector _connector;

		private Guid _userId;

		/// <summary>
		/// Конструктор запроса
		/// </summary>
		/// <param name="connector">Коннектор</param>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		public BookingServiceGetUserBookingsRequest(BookingServiceConnector connector, Guid userId)
		{
			_httpMethod = HttpMethod.Get;
			_route = "api/v1/user-bookings";

			_connector = connector;
			_userId = userId;
		}

		public override Connector GetConnector() => _connector;

		public override string BuildRoute()
		{
			return $"{_route}/{_userId}";
		}

		/// <summary>
		/// Возвращает уникальный идентификатор пользователя
		/// </summary>
		/// <returns>Уникальный идентификатор пользователя</returns>
		public Guid GetUserId() => _userId;

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Бронирования пользователя</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<IEnumerable<BookingServiceBookingModel>> Send()
		{
			return await _connector.Send(this);
		}
	}
}
