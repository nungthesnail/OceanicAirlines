using InterserviceCommunication.Connectors;
using InterserviceCommunication.Models.AuthenticationService;
using InterserviceCommunication.Exceptions;

namespace InterserviceCommunication.Requests.AuthenticationService
{
    /// <summary>
    /// Запрос создания пароля пользователя
    /// </summary>
    public class AuthenticationServiceCreatePasswordHashRequest : InterserviceRequest
    {
        private AuthenticationServiceConnector _connector;

        private AuthenticationServicePasswordModel _model;

        /// <summary>
        /// Конструктор запроса
        /// </summary>
        /// <param name="connector">Коннектор</param>
        /// <param name="model">Модель запроса</param>
        public AuthenticationServiceCreatePasswordHashRequest(
            AuthenticationServiceConnector connector,
            AuthenticationServicePasswordModel model)
        {
            _httpMethod = HttpMethod.Post;
            _route = "api/v1/hash";

            _connector = connector;
            _model = model;
        }

        public override Connector GetConnector() => _connector;

        public override string BuildRoute() => _route;

        /// <summary>
        /// Возвращает модель запроса
        /// </summary>
        /// <returns></returns>
        public AuthenticationServicePasswordModel GetModel() => _model;

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Созданный хеш пароля пользователя</returns>
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
