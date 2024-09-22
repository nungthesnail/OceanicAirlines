using InterserviceCommunication.Connectors;
using InterserviceCommunication.Exceptions;


namespace InterserviceCommunication.Requests.AuthenticationService
{
    /// <summary>
    /// Запрос проверки наличия пароля у пользователя
    /// </summary>
    public class AuthenticationServiceCheckUserHasPasswordHashRequest : InterserviceRequest
    {
        private AuthenticationServiceConnector _connector;

        private Guid _linkedUserId;

        /// <summary>
        /// Конструктор запроса
        /// </summary>
        /// <param name="connector">Конструктор</param>
        /// <param name="linkedUserId">Уникальный идентификатор пользователя</param>
        public AuthenticationServiceCheckUserHasPasswordHashRequest(
            AuthenticationServiceConnector connector, Guid linkedUserId)
        {
            _httpMethod = HttpMethod.Get;
            _route = "api/v1/check-user-has-password";

            _connector = connector;
            _linkedUserId = linkedUserId;
        }

        public override Connector GetConnector() => _connector;

        public override string BuildRoute()
        {
            return $"{_route}/{_linkedUserId}";
        }

        /// <summary>
        /// Возвращает уникальный идентификатор пользователя
        /// </summary>
        /// <returns></returns>
        public Guid GetLinkedUserId() => _linkedUserId;

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Результат проверки наличия у пользователя пароля</returns>
		/// <exception cref="RequestFailedException"></exception>
		/// <exception cref="InterserviceCommunicationException"></exception>
		/// <exception cref="UnathorizedException"></exception>
		/// <exception cref="ForbiddenException"></exception>
		/// <exception cref="NotFoundException"></exception>
		/// <exception cref="BadRequestException"></exception>
		public async Task<bool> Send()
        {
            return await _connector.Send(this);
        }
    }
}
