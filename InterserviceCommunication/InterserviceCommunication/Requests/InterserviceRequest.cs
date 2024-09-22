using InterserviceCommunication.Connectors;

namespace InterserviceCommunication.Requests
{
    /// <summary>
    /// Базовый класс межсервисного запроса
    /// </summary>
    public abstract class InterserviceRequest
    {
        protected HttpMethod _httpMethod = HttpMethod.Get;
        protected string _route = "";

		/// <summary>
		/// Возвращает коннектор запроса
		/// </summary>
		/// <returns>Коннектор запроса</returns>
		public abstract Connector GetConnector();

        /// <summary>
        /// Возвращает путь запроса
        /// </summary>
        /// <returns>Путь запроса</returns>
        public string GetRoute() => _route;

		/// <summary>
		/// Возвращает HTTP-метод запроса
		/// </summary>
		/// <returns>HTTP-метод запроса</returns>
		public HttpMethod GetHttpMethod() => _httpMethod;

        /// <summary>
        /// Строит полный путь запроса
        /// </summary>
        /// <returns>Построенный полный путь запроса</returns>
        public abstract string BuildRoute();
    }
}
