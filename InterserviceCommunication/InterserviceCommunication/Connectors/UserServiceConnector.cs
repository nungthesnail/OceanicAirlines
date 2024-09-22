using InterserviceCommunication.Requests.UserService;
using InterserviceCommunication.Models.UserService;
using InterserviceCommunication.Models;
using InterserviceCommunication.Exceptions;


namespace InterserviceCommunication.Connectors
{
	/// <summary>
	/// Коннектор микросервиса пользователей
	/// </summary>
	public sealed class UserServiceConnector : Connector
    {
        private UserServiceConnector()
        { }

		/// <summary>
		/// Конструктор коннектора микросервиса пользователей
		/// </summary>
		/// <param name="communicator">Межсервисный связист</param>
		/// <param name="settings">Настройки коннектора</param>
		public UserServiceConnector(InterserviceCommunicator communicator, ConnectorSettings settings)
        {
            _communicator = communicator;
            _settings = settings;
        }

		/// <summary>
		/// Отправляет запрос проверки существования пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Результат проверки существования пользователя</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<bool> Send(UserServiceCheckUserExistsRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();

            var httpResponse = await Send(method, route);

            var responseModel = await DeserializeHttpContent<BoolResponseModel>(httpResponse.Content);

            return responseModel!.Result;
        }

		/// <summary>
		/// Отправляет запрос получения пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Полученный пользователь</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<UserServiceUserModel> Send(UserServiceGetUserRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();

            var httpResponse = await Send(method, route);

            var responseModel = await DeserializeHttpContent<UserServiceUserModel>(httpResponse.Content);

            return responseModel!;
        }

		/// <summary>
		/// Отправляет запрос создания пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Созданный пользователь</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<UserServiceUserModel> Send(UserServiceCreateUserRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();
            var model = request.GetModel();

            var httpResponse = await Send(method, route, model);

            var responseModel = await DeserializeHttpContent<UserServiceUserModel>(httpResponse.Content);

            return responseModel!;
        }

		/// <summary>
		/// Отправляет запрос обновления пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Обновленный пользователь</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<UserServiceUserModel> Send(UserServiceUpdateUserRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();
            var model = request.GetModel();

            var httpResponse = await Send(method, route, model);

            var responseModel = await DeserializeHttpContent<UserServiceUserModel>(httpResponse.Content);

            return responseModel!;
        }

		/// <summary>
		/// Отправляет запрос удаления пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Удаленный пользователь</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<UserServiceUserModel> Send(UserServiceDeleteUserRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();

            var httpResponse = await Send(method, route);

            var responseModel = await DeserializeHttpContent<UserServiceUserModel>(httpResponse.Content);

            return responseModel!;
        }

		/// <summary>
		/// Создает запрос проверки существования пользователя
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Запрос проверки существования пользователя</returns>
		public UserServiceCheckUserExistsRequest CreateCheckUserExistsRequest(Guid? userId = null, string? userName = null)
        {
            return new UserServiceCheckUserExistsRequest(this, userId, userName);
        }

		/// <summary>
		/// Создает запрос получения пользователя
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <param name="userName">Имя пользователя</param>
		/// <returns>Запрос получения пользователя</returns>
		public UserServiceGetUserRequest CreateGetUserRequest(Guid? userId = null, string? userName = null)
        {
            return new UserServiceGetUserRequest(this, userId, userName);
        }

		/// <summary>
		/// Создает запрос создания пользователя
		/// </summary>
		/// <param name="model">Модель пользователя</param>
		/// <returns>Созданный пользователь</returns>
		public UserServiceCreateUserRequest CreateCreateUserRequest(UserServiceUserModel model)
        {
            return new UserServiceCreateUserRequest(this, model);
        }
    }
}
