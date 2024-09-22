using InterserviceCommunication.Models.UserService;
using InterserviceCommunication.Connectors;
using InterserviceCommunication.Exceptions;

namespace InterserviceCommunication.Requests.UserService
{
    /// <summary>
    /// Запрос получения пользователя
    /// </summary>
    public sealed class UserServiceGetUserRequest : InterserviceRequest
    {
        private UserServiceConnector _connector;

        private Guid? _userId;
        private string? _userName;

        /// <summary>
        /// Конструктор запроса
        /// </summary>
        /// <param name="connector">Коннектор</param>
        /// <param name="userId">Уникальный идентификатор пользователя</param>
        /// <param name="userName">Имя пользователя</param>
        public UserServiceGetUserRequest(UserServiceConnector connector, Guid? userId = null, string? userName = null)
        {
            _httpMethod = HttpMethod.Get;
            _route = "api/v1/user";

            _connector = connector;
            _userId = userId;
            _userName = userName;
        }

        public override Connector GetConnector() => _connector;

        public override string BuildRoute()
        {
            return (_userId != null) ? BuildRouteForId() : BuildRouteForName();
        }

        private string BuildRouteForId()
        {
            return $"{_route}/{_userId}";
        }

        private string BuildRouteForName()
        {
            return $"{_route}/{_userName}";
        }

		/// <summary>
		/// Возвращает уникальный идентификатор пользователя
		/// </summary>
		/// <returns>Уникальный идентификатор пользователя</returns>
		public Guid? GetUserId() => _userId;

		/// <summary>
		/// Возвращает имя пользователя
		/// </summary>
		/// <returns>Имя пользователя</returns>
		public string? GetUserName() => _userName;

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Полученный пользователь</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<UserServiceUserModel> Send()
        {
            return await _connector.Send(this);
        }
    }
}
