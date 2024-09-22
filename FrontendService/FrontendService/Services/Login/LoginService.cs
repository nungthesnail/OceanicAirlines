using FrontendService.Models.Authorization;
using FrontendService.Services.Login.Exceptions;
using InterserviceCommunication;
using InterserviceCommunication.Exceptions;
using InterserviceCommunication.Models.UserService;


namespace FrontendService.Services.Login
{
	/// <summary>
	/// Сервис авторизации пользователя
	/// </summary>
	public class LoginService : ILoginService
	{
		private readonly InterserviceCommunicator _interserviceCommunicator;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="interserviceCommunicator">Межсервисный связист</param>
		public LoginService(InterserviceCommunicator interserviceCommunicator)
		{
			_interserviceCommunicator = interserviceCommunicator;
		}

		public async Task<string> Login(LoginModel model)
		{
			var username = model.Username;
			var password = model.Password;

			var userId = await GetUserIdByUsername(username);

			var result = await Authenticate(userId, password);

			return result;
		}

		private async Task<Guid> GetUserIdByUsername(string username)
		{
			var user = await GetUserByUsername(username);

			var id = GetUserIdFromModel(user);

			return id;
		}

		private async Task<UserServiceUserModel> GetUserByUsername(string username)
		{
			try
			{
				var userServiceConnector = _interserviceCommunicator.GetUserServiceConnector();
				var getUserRequest = userServiceConnector.CreateGetUserRequest(userName: username);
				var result = await getUserRequest.Send();

				return result;
			}
			catch (NotFoundException)
			{
				throw new LoginFailedException("User doesn\'t exists");
			}
		}

		private Guid GetUserIdFromModel(UserServiceUserModel model)
		{
			return model.Id!;
		}

		private async Task<string> Authenticate(Guid userId, string password)
		{
			try
			{
				var authConnector = _interserviceCommunicator.GetAuthenticationServiceConnector();
				var authRequest = authConnector.CreateAuthenticateRequest(userId, password);
				var result = await authRequest.Send();

				return result;
			}
			catch (BadRequestException)
			{
				throw new LoginFailedException("Incorrect password");
			}
		}
	}
}
