using SharedFunctionality.AspNetCore;
using EntityFrameworkLogic;
using FlightService.Repository;
using FlightService.Repository.SearchChainOfResponsibility;


namespace FlightService
{
    /// <summary>
    /// Класс для конфигурирования коллекции сервисов и компонентов middlewares приложения
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Метод конфигурирования сервисов приложения
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="config">Конфигурация приложения</param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.ConfigureBusinessLogic();

            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenWithAuth();

            services.ConfigureAuthentication(config);
            services.ConfigureAuthorization();

            services.ConfigureCaching(config, "flightservice");
        }

        private static void ConfigureBusinessLogic(this IServiceCollection services)
        {
			services.AddDbContext<ApplicationContext>();
			services.AddScoped<DatabaseAirportFacade>();
			services.AddScoped<DatabaseAirportsPairFacade>();
			services.AddScoped<DatabaseBaseFlightFacade>();
			services.AddScoped<DatabaseSheduledFlightFacade>();

			services.AddTransient<DatabaseSearcher>();
			services.AddTransient<ISearchChainOfResponsibility, SearchChainOfResponsibility>();
		}

        /// <summary>
        /// Метод конфигурирования компонентов middlewares приложения
        /// </summary>
        /// <param name="app">Объект веб-приложения</param>
        public static void ConfigureMiddlewares(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
