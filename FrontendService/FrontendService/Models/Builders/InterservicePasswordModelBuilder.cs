using InterserviceCommunication.Models.AuthenticationService;
using InterserviceCommunication.Requests.AuthenticationService;

namespace FrontendService.Models.Builders
{
	/// <summary>
	/// Строитель модели пароля для межсервисного взаимодействия
	/// </summary>
	public static class InterservicePasswordModelBuilder
	{
		/// <summary>
		/// Строит модель пароля для межсервисного взаимодействия из уникального идентификатора пользователя
		/// и пароля
		/// </summary>
		/// <param name="linkedUserId">Уникальный идентификатор пользователя</param>
		/// <param name="password">Пароль</param>
		/// <returns>Построенная модель пароля для межсервисного взаимодействия</returns>
		public static AuthenticationServicePasswordModel Build(Guid linkedUserId, string password)
		{
			return new AuthenticationServicePasswordModel
			{
				LinkedUserId = linkedUserId,
				Password = password
			};
		}
	}
}
