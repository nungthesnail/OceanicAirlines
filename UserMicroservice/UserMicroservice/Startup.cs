using SharedFunctionality.AspNetCore;
using BuisnessLogic;


namespace UserMicroservice
{
    /// <summary>
    /// Класс расширений IServiceCollection и WebApplication для конфигурирования сервисов и Middlewares
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Метод конфигурирования сервисов веб-приложения
        /// </summary>
        /// <param name="services">Объект коллекции сервисов</param>
        /// <param name="config">Конфигурация приложения</param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGenWithAuth();

            services.ConfigureAuthentication(config);
            services.ConfigureAuthorization();
            
            services.ConfigureCaching(config, "userservice");

            services.AddTransient<BuisnessLogicApiBuilder>();
        }

        /// <summary>
        /// Метод конфигурирования Middlewares веб-приложения
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

            app.Run();
        }
    }
}
