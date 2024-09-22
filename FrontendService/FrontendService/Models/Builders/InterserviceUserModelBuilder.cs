using FrontendService.Models.Authorization;
using InterserviceCommunication.Models.UserService;

namespace FrontendService.Models.Builders
{
	/// <summary>
	/// Строитель модели пользователя для межсервисного взаимодействия
	/// </summary>
	public static class InterserviceUserModelBuilder
	{
		/// <summary>
		/// Строит модель пользователя для межсервисного взаимодействия из модели данных для регистрации пользователя
		/// </summary>
		/// <param name="model">Данные, необходимые для регистрации пользователя</param>
		/// <returns>Построенная модель пользователя для межсервисного взаимодействия</returns>
		public static UserServiceUserModel BuildFrom(RegisterModel model)
		{
			return new UserServiceUserModel
			{
				Name = model.Username,
				Email = model.Email,
				CreatedAt = DateTime.UtcNow,
				Role = "User"
			};
		}
	}
}
