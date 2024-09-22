using InterserviceCommunication.Models.BookingService;
using InterserviceCommunication.Models;
using InterserviceCommunication.Requests.BookingService;
using InterserviceCommunication.Exceptions;


namespace InterserviceCommunication.Connectors
{
	/// <summary>
	/// Коннектор микросервиса бронирований
	/// </summary>
	public class BookingServiceConnector : Connector
	{
		private BookingServiceConnector()
		{ }

		/// <summary>
		/// Конструктор коннектора микросервиса бронирований
		/// </summary>
		/// <param name="communicator">Межсервисный связист</param>
		/// <param name="settings">Настройки коннектора</param>
		public BookingServiceConnector(InterserviceCommunicator communicator, ConnectorSettings settings)
		{
			_communicator = communicator;
			_settings = settings;
		}

		/// <summary>
		/// Отправляет запрос бронирования
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Результат бронирования</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<BookingServiceBookingModel> Send(BookingServiceBookRequest request)
		{
			var method = request.GetHttpMethod();
			var route = request.BuildRoute();
			var model = request.GetModel();

			var result = await Send(method, route, model);

			var responseModel = await DeserializeHttpContent<BookingServiceBookingModel>(result.Content);

			return responseModel!;
		}

		/// <summary>
		/// Отправляет запрос получения бронирований пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Бронирования пользователя</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<IEnumerable<BookingServiceBookingModel>> Send(BookingServiceGetUserBookingsRequest request)
		{
			var method = request.GetHttpMethod();
			var route = request.BuildRoute();

			var result = await Send(method, route);

			var responseModel = await DeserializeHttpContent<EnumerableResponseModel<BookingServiceBookingModel>>(result.Content);

			return responseModel!.Result;
		}

		/// <summary>
		/// Создает запрос бронирования
		/// </summary>
		/// <param name="model">Модель бронирования</param>
		/// <returns>Запрос бронирования</returns>
		public BookingServiceBookRequest CreateBookRequest(BookingServiceBookingModel model)
		{
			return new BookingServiceBookRequest(this, model);
		}

		/// <summary>
		/// Создает запрос получения бронирований пользователя
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Запрос получения бронирований пользователя</returns>
		public BookingServiceGetUserBookingsRequest CreateGetUserBookingsRequest(Guid userId)
		{
			return new BookingServiceGetUserBookingsRequest(this, userId);
		}
	}
}
