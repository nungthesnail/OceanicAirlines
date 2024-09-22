using NotificationService.Services.Mailing;
using NotificationService.Services.NotificationProviders;
using NotificationService.Services.RabbitMq;
using NotificationService.Services.SenderService;
using SharedFunctionality.AspNetCore;


namespace NotificationService
{
	public static class Startup
	{
		public static async Task ConfigureServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddControllers();
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGenWithAuth();

			await services.AddInterserviceCommunicator(config);

			services.ConfigureAuthentication(config);
			services.ConfigureAuthorization();

			services.ConfigureCaching(config, "notificationservice");

			services.ConfigureBusinessLogic();
		}

		private static void ConfigureBusinessLogic(this IServiceCollection services)
		{
			services.AddHostedService<RabbitMqListener>();

			services.AddTransient<IMailingService, MailingService>();
			services.AddTransient<MailMessageBuilder>();
			services.AddTransient<EmailNotificationProvider>();
			services.AddTransient<UserIdNotificationProvider>();
			services.AddTransient<ISenderService, SenderService>();
			services.AddTransient<NotificationProviderFactory>();
		}

		public static void ConfigureMiddlewares(this WebApplication app)
		{
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();
		}
	}
}
