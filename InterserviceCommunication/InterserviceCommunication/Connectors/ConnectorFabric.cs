namespace InterserviceCommunication.Connectors
{
    /// <summary>
    /// Фабрика коннекторов микросервисов
    /// </summary>
    public static class ConnectorFabric
    {
		/// <summary>
		/// Создает коннектор микросервиса аутентификации и авторизации
		/// </summary>
		/// <param name="communicator">Межсервисный связист</param>
		/// <param name="serviceUrl">Адрес микросервиса</param>
		/// <returns>Коннектор микросервиса аутентификации и авторизации</returns>
		public static AuthenticationServiceConnector CreateAuthenticationServiceConnector(
            InterserviceCommunicator communicator, string serviceUrl)
        {
            var settings = new ConnectorSettings()
            {
                ServiceUrl = serviceUrl
            };
            return new AuthenticationServiceConnector(communicator, settings);
        }

		/// <summary>
		/// Создает коннектор микросервиса пользователей
		/// </summary>
		/// <param name="communicator">Межсервисный связист</param>
		/// <param name="serviceUrl">Адрес микросервиса</param>
		/// <returns>Коннектор микросервиса пользователей</returns>
		public static UserServiceConnector CreateUserServiceConnector(
            InterserviceCommunicator communicator, string serviceUrl)
        {
            var settings = new ConnectorSettings()
            {
                ServiceUrl = serviceUrl
            };
            return new UserServiceConnector(communicator, settings);
        }

		/// <summary>
		/// Создает коннектор микросервиса рейсов
		/// </summary>
		/// <param name="communicator">Межсервисный связист</param>
		/// <param name="serviceUrl">Адрес микросервиса</param>
		/// <returns>Коннектор микросервиса рейсов</returns>
		public static FlightServiceConnector CreateFlightServiceConnector(
            InterserviceCommunicator communicator, string serviceUrl)
        {
            var settings = new ConnectorSettings()
            {
                ServiceUrl = serviceUrl
            };
            return new FlightServiceConnector(communicator, settings);
        }

		/// <summary>
		/// Создает коннектор микросервиса бронирования
		/// </summary>
		/// <param name="communicator">Межсервисный связист</param>
		/// <param name="serviceUrl">Адрес микросервиса</param>
		/// <returns>Коннектор микросервиса бронирования</returns>
		public static BookingServiceConnector CreateBookingServiceConnector(
			InterserviceCommunicator communicator, string serviceUrl)
		{
			var settings = new ConnectorSettings()
			{
				ServiceUrl = serviceUrl
			};
			return new BookingServiceConnector(communicator, settings);
		}
	}
}
