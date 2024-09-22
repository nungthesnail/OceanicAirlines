using InterserviceCommunication.Connectors.Exceptions.AuthenticationService;
using InterserviceCommunication.Connectors;
using InterserviceCommunication.Exceptions;
using System.Diagnostics;


namespace InterserviceCommunication.Authentication
{
    /// <summary>
    /// Менеджер идентификации межсервисного связиста
    /// </summary>
    internal class AuthenticationManager
    {
        private Guid _id { get; set; }
        private string _password { get; set; } = null!;

        private bool _doAuthentication { get; set; } = true;

        private AuthenticationServiceConnector _authenticationServiceConnector = null!;

        private string _token = String.Empty;

        private bool _isAuthenticated = false;

        private AuthenticationManager()
        { }

        private AuthenticationManager(AuthenticationSettings settings)
        {
            InitializeFields(settings);
        }

        private void InitializeFields(AuthenticationSettings settings)
        {
            _id = settings.Id;
            _password = settings.Password;
            _doAuthentication = settings.DoAuthentication;
            _authenticationServiceConnector = settings.AuthenticationServiceConnector;
        }

        /// <summary>
        /// Авторизует межсервисного связиста
        /// </summary>
        /// <returns></returns>
        /// <exception cref="AuthenticationFailedException"></exception>
        public async Task Authorize()
        {
            try
            {
                _isAuthenticated = true;

                await TryToAuthorize();
            }
            catch (NotFoundException)
            {
                _isAuthenticated = false;

                throw new AuthenticationFailedException($"Error: 404 - Not Found");
            }
            catch (BadRequestException)
            {
                _isAuthenticated = false;

                throw new AuthenticationFailedException($"Error: 400 - Bad Request");
            }
        }

        private async Task TryToAuthorize()
        {
            if (_doAuthentication)
            {
                var authenticateRequest = _authenticationServiceConnector.CreateAuthenticateRequest(_id, _password);

                var token = await authenticateRequest.Send();

                SetToken(token);
            }
        }

        private void SetToken(string? token)
        {
            _token = token ?? String.Empty;
        }

        /// <summary>
        /// Возвращает JSON Web Token авторизации межсервисного связиста
        /// </summary>
        /// <returns></returns>
        internal string GetToken() => _token;

        /// <summary>
        /// Включает процедуру идентификации
        /// </summary>
        public void EnableDoingAuthentication()
        {
            _doAuthentication = true;
        }

        /// <summary>
        /// Выключает процедуру идентификации
        /// </summary>
        public void DisableDoingAuthentication()
        {
            _doAuthentication = false;
        }

		/// <summary>
		/// Выполняет ли межсервисный связист процеруду идентификации
		/// </summary>
		/// <returns>Выполняет ли межсервисный связист процеруду идентификации</returns>
		public bool DoingAuthentication() => _doAuthentication;

		/// <summary>
		/// Идентифицирован ли межсервисный связист
		/// </summary>
		/// <returns>Идентифицирован ли межсервисный связист</returns>
		public bool IsAuthenticated() => _isAuthenticated;

        /// <summary>
        /// Создает менеджера идентификации
        /// </summary>
        /// <param name="settings">Настройки идентификации</param>
        /// <returns>Созданный менеджер идентификации</returns>
        public static AuthenticationManager CreateAuthenticationManager(AuthenticationSettings settings)
        {
            var manager = new AuthenticationManager(settings);

            return manager;
        }
    }
}
