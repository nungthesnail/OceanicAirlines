using InterserviceCommunication.Connectors;
using InterserviceCommunication.Models.AuthenticationService;
using InterserviceCommunication.Exceptions;

namespace InterserviceCommunication.Requests.AuthenticationService
{
    /// <summary>
    /// Запрос удаления хеша пароля пользователя
    /// </summary>
    public class AuthenticationServiceDeletePasswordHashRequest : InterserviceRequest
    {
        private AuthenticationServiceConnector _connector;

        private Guid _linkedUserId;

        /// <summary>
        /// Конструктор запроса
        /// </summary>
        /// <param name="connector">Коннектор</param>
        /// <param name="linkedUserId">Уникальный идентификатор пользователя</param>
        public AuthenticationServiceDeletePasswordHashRequest(
            AuthenticationServiceConnector connector, Guid linkedUserId)
        {
            _httpMethod = HttpMethod.Delete;
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
		/// Возвращает уникальный идентификатор пользователя
		/// </summary>
		/// <returns>Уникальный идентификатор пользователя</returns>
		public Guid GetLinkedUserId() => _linkedUserId;

		/// <summary>
		/// Отправляет запрос
		/// </summary>
		/// <returns>Удаленный хеш пароля пользователя</returns>
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
