using InterserviceCommunication.Models.AuthenticationService;
using InterserviceCommunication.Connectors;
using InterserviceCommunication.Exceptions;


namespace InterserviceCommunication.Requests.AuthenticationService
{
    /// <summary>
    /// Запрос аутентификации и авторизации
    /// </summary>
    public sealed class AuthenticationServiceAuthenticateRequest : InterserviceRequest
    {
        private AuthenticationServiceConnector _connector;

        private AuthenticationServiceAuthenticationModel _model;

        /// <summary>
        /// Конструктор запроса
        /// </summary>
        /// <param name="connector">Коннектор</param>
        /// <param name="model">Модель запроса</param>
        public AuthenticationServiceAuthenticateRequest(
            AuthenticationServiceConnector connector,
            AuthenticationServiceAuthenticationModel model)
        {
            _httpMethod = HttpMethod.Post;
            _route = "api/v1/authenticate";

            _connector = connector;
            _model = model;
        }

        public override Connector GetConnector() => _connector;

        public override string BuildRoute() => _route;

        /// <summary>
        /// Возвращает модель
        /// </summary>
        /// <returns>Модель запроса</returns>
        public AuthenticationServiceAuthenticationModel GetModel() => _model;

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Результат прохождения процедуры аутентификации и авторизации в формате JSON Web Token</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<string> Send()
        {
            return await _connector.Send(this);
        }
    }
}
