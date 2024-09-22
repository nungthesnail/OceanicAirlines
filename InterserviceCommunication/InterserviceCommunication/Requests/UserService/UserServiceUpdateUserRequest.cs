using InterserviceCommunication.Connectors;
using InterserviceCommunication.Models.UserService;
using InterserviceCommunication.Exceptions;

namespace InterserviceCommunication.Requests.UserService
{
    /// <summary>
    /// Запрос обновления пользователя
    /// </summary>
    public sealed class UserServiceUpdateUserRequest : InterserviceRequest
    {
        private UserServiceConnector _connector;

        private UserServiceUserModel _model;

        /// <summary>
        /// Конструктор запроса
        /// </summary>
        /// <param name="connector">Коннектор</param>
        /// <param name="model">Модель запроса</param>
        public UserServiceUpdateUserRequest(UserServiceConnector connector, UserServiceUserModel model)
        {
            _httpMethod = HttpMethod.Put;
            _route = "api/v1/user";

            _connector = connector;
            _model = model;
        }

        public override Connector GetConnector() => _connector;

        public override string BuildRoute() => _route;

		/// <summary>
		/// Возвращает модель запроса
		/// </summary>
		/// <returns>Модель запроса</returns>
		public UserServiceUserModel GetModel() => _model;

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Обновленный пользователь</returns>
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
