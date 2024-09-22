using InterserviceCommunication.Authentication;
using InterserviceCommunication.Connectors;
using Microsoft.Extensions.Configuration;


namespace InterserviceCommunication
{
    /// <summary>
    /// Межсервисный связист. Обеспечивает межсервисное взаимодействие
    /// </summary>
    public class InterserviceCommunicator
    {
        private readonly IConfiguration _config;

        private AuthenticationManager _authenticationManager = null!;

        private AuthenticationServiceConnector _authenticationServiceConnector = null!;
        private UserServiceConnector _userServiceConnector = null!;
        private FlightServiceConnector _flightServiceConnector = null!;
        private BookingServiceConnector _bookingServiceConnector = null!;

		private InterserviceCommunicator(IConfiguration config)
		{
            _config = config;

			InitializeConnectors();
		}

		private void InitializeConnectors()
        {
            InitializeAuthenticationServiceConnector();
            InitializeUserServiceConnector();
            InitializeFlightServiceConnector();
            InitializeBookingServiceConnector();
        }

        private void InitializeAuthenticationServiceConnector()
        {
            var serviceAddress = _config["Microservices:AuthenticationService:Address"] ?? "";

            _authenticationServiceConnector = ConnectorFabric.CreateAuthenticationServiceConnector(this, serviceAddress);
        }

        private void InitializeUserServiceConnector()
		{
			var serviceAddress = _config["Microservices:UserService:Address"] ?? "";

			_userServiceConnector = ConnectorFabric.CreateUserServiceConnector(this, serviceAddress);
        }

        private void InitializeFlightServiceConnector()
		{
			var serviceAddress = _config["Microservices:FlightService:Address"] ?? "";

			_flightServiceConnector = ConnectorFabric.CreateFlightServiceConnector(this, serviceAddress);
        }

        private void InitializeBookingServiceConnector()
		{
			var serviceAddress = _config["Microservices:BookingService:Address"] ?? "";

			_bookingServiceConnector = ConnectorFabric.CreateBookingServiceConnector(this, serviceAddress);
        }

        private void InitializeDefaultAuthenticationManager()
        {
            var defaultAuthenticationSettings = new AuthenticationSettings()
            {
                Id = Guid.NewGuid(),
                DoAuthentication = false,
                AuthenticationServiceConnector = _authenticationServiceConnector
            };
            _authenticationManager = AuthenticationManagerBuilder.Build(defaultAuthenticationSettings);
        }

        private void InitializeAuthenticationManager(InterserviceCommunicatorSettings settings)
        {
            var authenticationSettings = new AuthenticationSettings()
            {
                Id = settings.ServiceId,
                Password = settings.ServicePassword,
                DoAuthentication = settings.DoAuthentication,
                AuthenticationServiceConnector = _authenticationServiceConnector
            };
            _authenticationManager = AuthenticationManagerBuilder.Build(authenticationSettings);
        }

        /// <summary>
        /// Запрашивает авторизацию межсервисного связиста
        /// </summary>
        /// <returns></returns>
        public async Task RequestAuthorization()
        {
            await _authenticationManager.Authorize();
        }

        /// <summary>
        /// Проверяет авторизацию межсервисного связиста
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return _authenticationManager.IsAuthenticated();
        }

		/// <summary>
		/// Возвращает коннектор для микросервиса пользователей
		/// </summary>
		/// <returns>Коннектор для микросервиса пользователей</returns>
		public UserServiceConnector GetUserServiceConnector() => _userServiceConnector;

		/// <summary>
		/// Возвращает коннектор для микросервиса аутентификации и авторизации
		/// </summary>
		/// <returns>Коннектор для микросервиса аутентификации и авторизации</returns>
		public AuthenticationServiceConnector GetAuthenticationServiceConnector() => _authenticationServiceConnector;

		/// <summary>
		/// Возвращает коннектор для микросервиса рейсов
		/// </summary>
		/// <returns>Коннектор для микросервиса рейсов</returns>
		public FlightServiceConnector GetFlightServiceConnector() => _flightServiceConnector;

		/// <summary>
		/// Возвращает коннектор для микросервиса бронирований
		/// </summary>
		/// <returns>Коннектор для микросервиса бронирований</returns>
		public BookingServiceConnector GetBookingServiceConnector() => _bookingServiceConnector;

        /// <summary>
        /// Запрашивает JSON Web Token авторизации межсервисного связиста
        /// </summary>
        /// <returns></returns>
        internal string RequestAuthorizationToken()
        {
            return _authenticationManager.GetToken();
        }

		/// <summary>
		/// Создает экземпляр межсервисного связиста
		/// </summary>
		/// <param name="settings">Настройки межсервисного связиста</param>
		/// <param name="config">Конфигурация приложения</param>
		/// <returns>Экземпляр межсервисного связиста</returns>
		public static async Task<InterserviceCommunicator> CreateInterserviceCommunicator(
            InterserviceCommunicatorSettings settings,
            IConfiguration config)
        {
            var communicator = new InterserviceCommunicator(config);

            communicator.InitializeAuthenticationManager(settings);

            if (settings.AuthenticateImmediately)
            {
                await communicator.RequestAuthorization();
            }

            return communicator;
        }
    }
}
