using BookingService.BookingLogic;
using BookingService.BookingLogic.Validation;
using BookingService.Repository;
using BookingService.Services.BookingCodeGenerator;
using BookingService.Services.NotificationSender;
using BookingService.Services.RabbitMq;
using EntityFrameworkLogic;
using SharedFunctionality.AspNetCore;


namespace BookingService
{
    /// <summary>
    /// Класс расширений для конфигурации коллекции сервисов и компонентов middlewares
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Конфигурирует коллекцию сервисов
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="config">Конфигурация приложения</param>
        /// <returns></returns>
        public static async Task ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenWithAuth();

            await services.AddInterserviceCommunicator(config);

            services.ConfigureAuthentication(config);
            services.ConfigureAuthorization();

            services.ConfigureCaching(config, "bookingservice");

            services.ConfigureBuisnessLogic();
        }

        private static void ConfigureBuisnessLogic(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>();

            services.AddTransient<DatabaseBookingFacade>();
            services.AddTransient<DatabasePassengerFacade>();
            services.AddTransient<DatabasePassengerToBookingFacade>();

            services.AddTransient<BookingManagerService>();
            services.AddTransient<BookingValidator>();

            services.AddTransient<IBookingCodeGeneratorService, BookingCodeGeneratorService>();

            services.AddTransient<IRabbitMqService, RabbitMqService>();
            services.AddTransient<NotificationBuilder>();
            services.AddTransient<INotificationSenderService, NotificationSenderService>();
        }

        /// <summary>
        /// Конфигурирует компоненты middlewares веб-приложения
        /// </summary>
        /// <param name="app">Объект веб-приложения</param>
        public static void ConfigureMiddlewares(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseServiceAuthorization();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
