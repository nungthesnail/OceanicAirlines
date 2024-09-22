using FrontendService.Services.Flights;
using FrontendService.Services.Login;
using FrontendService.Services.Search;
using FrontendService.Services.Booking;
using SharedFunctionality.AspNetCore;
using FrontendService.Services.Register;


namespace FrontendService
{
	/// <summary>
	/// Класс для конфигурации коллекции сервисов и компонентов middlewares веб-приложения
	/// </summary>
	public static class Startup
    {
        /// <summary>
        /// Конфигурирует коллекцию сервисов приложения
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="config">Конфигурация приложения</param>
        /// <returns></returns>
        public static async Task ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews();

            await services.AddInterserviceCommunicator(config);

            services.ConfigureAuthentication(config);
            services.ConfigureAuthorization();

            services.ConfigureBusinessLogicServices();
        }

        private static void ConfigureBusinessLogicServices(this IServiceCollection services)
        {
            services.AddTransient<IFlightSearchService, FlightSearchService>();
            services.AddTransient<ILoginService, LoginService>();
            services.AddTransient<IGetFlightService, GetFlightService>();
            services.AddTransient<IBookingViewModelBuilderService, BookingViewModelBuilderService>();
            services.AddTransient<IUserBookingsProviderService, UserBookingsProviderService>();
            services.AddTransient<IRegisterProviderService, RegisterProviderService>();
        }

        /// <summary>
        /// Конфигурирует компоненты middlewares веб-приложения
        /// </summary>
        /// <param name="app">Объект веб-приложения</param>
        public static void ConfigureMiddlewares(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseServiceAuthorization();

            app.UseStatusCodePagesWithReExecute("/page/error", "?code={0}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/");
        }
    }
}
