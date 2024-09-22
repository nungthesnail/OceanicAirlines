using InterserviceCommunication.Connectors;
using InterserviceCommunication.Models.AuthenticationService;
using InterserviceCommunication.Exceptions;

namespace InterserviceCommunication.Requests.AuthenticationService
{
    /// <summary>
    /// Запрос получения хеша пароля пользователя
    /// </summary>
    public class AuthenticationServiceGetPasswordHashRequest : InterserviceRequest
    {
        private AuthenticationServiceConnector _connector;

        private Guid _linkedUserId;

        /// <summary>
        /// Конструктор запроса
        /// </summary>
        /// <param name="connector">Коннектор</param>
        /// <param name="linkedUserId">Уникальный идентификатор пользователя</param>
        public AuthenticationServiceGetPasswordHashRequest(
            AuthenticationServiceConnector connector, Guid linkedUserId)
        {
            _httpMethod = HttpMethod.Get;
            _route = "api/v1/hash";

            _connector = connector;
            _linkedUserId = linkedUserId;
        }

        public override Connector GetConnector() => _connector;

        public override string BuildRoute()
        {
            return $"{_route}/{_linkedUserId}";
        }

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Хеш пароля пользователя</returns>
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
