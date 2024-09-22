using FrontendService.Models.Authorization;
using FrontendService.Models.Builders;
using FrontendService.Services.Register.Exceptions;
using InterserviceCommunication;
using InterserviceCommunication.Exceptions;
using InterserviceCommunication.Models.AuthenticationService;
using InterserviceCommunication.Models.UserService;


namespace FrontendService.Services.Register
{
	/// <summary>
	/// Сервис, осуществляющий регистрацию пользователя
	/// </summary>
	public class RegisterProviderService : IRegisterProviderService
	{
		private readonly InterserviceCommunicator _interserviceCommunicator;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="interserviceCommunicator">Межсервисный связист</param>
		public RegisterProviderService(InterserviceCommunicator interserviceCommunicator)
		{
			_interserviceCommunicator = interserviceCommunicator;
		}

		public async Task Register(RegisterModel model)
		{
			ThrowIfPasswordAndRepeatArentEqual(model.Password, model.PasswordRepeat);

			var userModel = CreateUserModel(model);

			var createdUser = await SendCreationRequestToUserService(userModel);

			var userId = createdUser.Id;

			var passwordModel = CreatePasswordModel(userId, model.Password);

			await SendCreationRequestToAuthService(passwordModel);
		}

		private void ThrowIfPasswordAndRepeatArentEqual(string password, string repeat)
		{
			if (password != repeat)
			{
				throw new RegisterFailedException();
			}
		}

		private UserServiceUserModel CreateUserModel(RegisterModel model)
		{
			return InterserviceUserModelBuilder.BuildFrom(model);
		}

		private async Task<UserServiceUserModel> SendCreationRequestToUserService(UserServiceUserModel userModel)
		{
			try
			{
				var userServiceConnector = _interserviceCommunicator.GetUserServiceConnector();
				var request = userServiceConnector.CreateCreateUserRequest(userModel);
				var result = await request.Send();

				return result;
			}
			catch (InterserviceCommunicationException)
			{
				throw new RegisterFailedException();
			}
		}

		private AuthenticationServicePasswordModel CreatePasswordModel(Guid userId, string password)
		{
			return InterservicePasswordModelBuilder.Build(userId, password);
		}

		private async Task SendCreationRequestToAuthService(AuthenticationServicePasswordModel pwdModel)
		{
			try
			{
				var authServiceConnector = _interserviceCommunicator.GetAuthenticationServiceConnector();
				var request = authServiceConnector.CreateCreatePasswordHashRequest(pwdModel);
				var createdPassword = await request.Send();
			}
			catch (InterserviceCommunicationException)
			{
				throw new RegisterFailedException();
			}
		}
	}
}
