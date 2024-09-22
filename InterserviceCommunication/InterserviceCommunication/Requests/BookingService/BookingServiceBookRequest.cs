using InterserviceCommunication.Connectors;
using InterserviceCommunication.Models.BookingService;
using InterserviceCommunication.Exceptions;


namespace InterserviceCommunication.Requests.BookingService
{
	/// <summary>
	/// Запрос бронирования
	/// </summary>
	public class BookingServiceBookRequest : InterserviceRequest
	{
		private BookingServiceConnector _connector;

		private BookingServiceBookingModel _model;

		/// <summary>
		/// Конструктор запроса
		/// </summary>
		/// <param name="connector">Коннектор</param>
		/// <param name="model">Модель запроса</param>
		public BookingServiceBookRequest(BookingServiceConnector connector, BookingServiceBookingModel model)
		{
			_httpMethod = HttpMethod.Post;
			_route = "api/v1/booking";

			_connector = connector;
			_model = model;
		}

		public override Connector GetConnector() => _connector;

		public override string BuildRoute() => _route;

		/// <summary>
		/// Возвращает модель запроса
		/// </summary>
		/// <returns>Модель запроса</returns>
		public BookingServiceBookingModel GetModel() => _model;

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Результат бронирования</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<BookingServiceBookingModel> Send()
		{
			return await _connector.Send(this);
		}
	}
}
