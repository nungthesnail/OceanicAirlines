using InterserviceCommunication.Connectors;
using InterserviceCommunication.Models.AuthenticationService;
using InterserviceCommunication.Exceptions;

namespace InterserviceCommunication.Requests.AuthenticationService
{
    /// <summary>
    /// Запрос обновления хеша пароля пользователя
    /// </summary>
    public class AuthenticationServiceUpdatePasswordHashRequest : InterserviceRequest
    {
        private AuthenticationServiceConnector _connector;

        private AuthenticationServicePasswordHashModel _model;

        /// <summary>
        /// Конструктор запроса
        /// </summary>
        /// <param name="connector">Коннектор</param>
        /// <param name="model">Модель запроса</param>
        public AuthenticationServiceUpdatePasswordHashRequest(
            AuthenticationServiceConnector connector,
            AuthenticationServicePasswordHashModel model)
        {
            _httpMethod = HttpMethod.Put;
            _route = "api/v1/hash";

            _connector = connector;
            _model = model;
        }

        public override Connector GetConnector() => _connector;

        public override string BuildRoute() => _route;

        /// <summary>
        /// Возвращает модель запроса
        /// </summary>
        /// <returns>Модель запроса</returns>
        public AuthenticationServicePasswordHashModel GetModel() => _model;

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Обновленный хеш пароля пользователя</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<AuthenticationServicePasswordHashModel> Send()
        {
            return await _connector.Send(this);
        }
    }
}
