using InterserviceCommunication.Requests.AuthenticationService;
using InterserviceCommunication.Models.AuthenticationService;
using InterserviceCommunication.Models;
using InterserviceCommunication.Exceptions;

namespace InterserviceCommunication.Connectors
{
    /// <summary>
    /// Коннектор микросервиса аутентификации и авторизации
    /// </summary>
    public sealed class AuthenticationServiceConnector : Connector
    {
        private AuthenticationServiceConnector()
        { }

        /// <summary>
        /// Конструктор коннектора микросервиса аутентификации и авторизации
        /// </summary>
        /// <param name="communicator">Межсервисный связист</param>
        /// <param name="settings">Настройки коннектора</param>
        public AuthenticationServiceConnector(InterserviceCommunicator communicator, ConnectorSettings settings)
        {
            _communicator = communicator;
            _settings = settings;
        }

		/// <summary>
		/// Отправляет запрос прохождения процедуры аутентификации и авторизации
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Результат прохождения процедуры аутентификации и авторизации в формате JSON Web Token</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<string> Send(AuthenticationServiceAuthenticateRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();
            var model = request.GetModel();

            var result = await Send(method, route, model);

            var responseModel = await DeserializeHttpContent<StringResponseModel>(result.Content);

            return responseModel!.Result;
        }

		/// <summary>
		/// Отправляет запрос проверки наличия у пользователя пароля
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Результат проверки наличия у пользователя пароля</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<bool> Send(AuthenticationServiceCheckUserHasPasswordHashRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();

            var result = await Send(method, route);

            var responseModel = await DeserializeHttpContent<BoolResponseModel>(result.Content);

            return responseModel!.Result;
        }

		/// <summary>
		/// Отправляет запрос создания пароля у пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Созданный хеш пароля пользователя</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<AuthenticationServicePasswordHashModel> Send
            (AuthenticationServiceCreatePasswordHashRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();
            var model = request.GetModel();

            var result = await Send(method, route, model);

            var responseModel = await DeserializeHttpContent<AuthenticationServicePasswordHashModel>(result.Content);

            return responseModel!;
        }

		/// <summary>
		/// Отправляет запрос получения хеша пароля пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Хеш пароля пользователя</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<AuthenticationServicePasswordHashModel> Send
            (AuthenticationServiceGetPasswordHashRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();

            var result = await Send(method, route);

            var responseModel = await DeserializeHttpContent<AuthenticationServicePasswordHashModel>(result.Content);

            return responseModel!;
        }

		/// <summary>
		/// Отправляет запрос обновления хеша пароля пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Обновленный хеш пароля пользователя</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<AuthenticationServicePasswordHashModel> Send
            (AuthenticationServiceUpdatePasswordHashRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();
            var model = request.GetModel();

            var result = await Send(method, route, model);

            var responseModel = await DeserializeHttpContent<AuthenticationServicePasswordHashModel>(result.Content);

            return responseModel!;
        }

		/// <summary>
		/// Отправляет запрос удаления хеша пароля пользователя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <returns>Удаленный хеш пароля пользователя</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<AuthenticationServicePasswordHashModel> Send
            (AuthenticationServiceDeletePasswordHashRequest request)
        {
            var method = request.GetHttpMethod();
            var route = request.BuildRoute();

            var result = await Send(method, route);

            var responseModel = await DeserializeHttpContent<AuthenticationServicePasswordHashModel>(result.Content);

            return responseModel!;
        }

		/// <summary>
		/// Создает запрос прохождения процедуры аутентификации и авторизации
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <param name="password">Пароль</param>
		/// <returns>Запрос прохождения процедуры аутентификации и авторизации</returns>
		public AuthenticationServiceAuthenticateRequest CreateAuthenticateRequest(Guid userId, string password)
        {
            var model = CreateAuthenticationModel(userId, password);

            return CreateAuthenticateRequest(model);
        }

        private AuthenticationServiceAuthenticationModel CreateAuthenticationModel(Guid userId, string password)
        {
            return new AuthenticationServiceAuthenticationModel()
            {
                Id = userId,
                Password = password
            };
        }

		/// <summary>
		/// Создает запрос прохождения процедуры аутентификации и авторизации
		/// </summary>
		/// <param name="model">Модель данных, необходимых для авторизации</param>
		/// <returns>Запрос прохождения процедуры аутентификации и авторизации</returns>
		public AuthenticationServiceAuthenticateRequest CreateAuthenticateRequest
            (AuthenticationServiceAuthenticationModel model)
        {
            return new AuthenticationServiceAuthenticateRequest(this, model);
        }

		/// <summary>
		/// Создает запрос проверки наличия у пользователя пароля
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Запрос проверки наличия у пользователя пароля</returns>
		public AuthenticationServiceCheckUserHasPasswordHashRequest CreateCheckUserHasPasswordHashRequest
            (Guid userId)
        {
            return new AuthenticationServiceCheckUserHasPasswordHashRequest(this, userId);
        }

		/// <summary>
		/// Создает запрос создания пароля пользователя
		/// </summary>
		/// <param name="model">Модель пароля пользователя</param>
		/// <returns>Запрос создания пароля пользователя</returns>
		public AuthenticationServiceCreatePasswordHashRequest CreateCreatePasswordHashRequest
            (AuthenticationServicePasswordModel model)
        {
            return new AuthenticationServiceCreatePasswordHashRequest(this, model);
        }

		/// <summary>
		/// Создает запрос получения хеша пароля пользователя
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Запрос получения хеша пароля пользователя</returns>
		public AuthenticationServiceGetPasswordHashRequest CreateGetPasswordHashRequest
            (Guid userId)
        {
            return new AuthenticationServiceGetPasswordHashRequest(this, userId);
        }

		/// <summary>
		/// Создает запрос обновления хеша пароля пользователя
		/// </summary>
		/// <param name="model">Модель пароля пользователя</param>
		/// <returns>Запрос обновления хеша пароля пользователя</returns>
		public AuthenticationServiceUpdatePasswordHashRequest CreateUpdatePasswordHashRequest
            (AuthenticationServicePasswordHashModel model)
        {
            return new AuthenticationServiceUpdatePasswordHashRequest(this, model);
        }

		/// <summary>
		/// Создает запрос удаления хеша пароля пользователя
		/// </summary>
		/// <param name="userId">Уникальный идентификатор пользователя</param>
		/// <returns>Запрос удаления хеша пароля пользователя</returns>
		public AuthenticationServiceDeletePasswordHashRequest CreateDeletePasswordHashRequest
            (Guid userId)
        {
            return new AuthenticationServiceDeletePasswordHashRequest(this, userId);
        }
    }
}
