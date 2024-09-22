using JwtUtils;
using EntityFrameworkLogic;
using BuisnessLogic.Api;
using BuisnessLogic.Api.Management;
using BuisnessLogic.Api.Authentication;
using BuisnessLogic.Handlers.Management;
using BuisnessLogic.Handlers.Authentication;
using BuisnessLogic.Repository;
using PasswordUtils;
using SharedFunctionality.AspNetCore;


namespace AuthenticationService
{
    /// <summary>
    /// Класс расширений IServiceCollection и WebApplication
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Метод конфигурирования сервисов веб-приложения
        /// </summary>
        /// <param name="services">Объект коллекции сервисов</param>
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

            services.ConfigureCaching(config, "authservice");

            services.ConfigureBuisnessLogic();
        }

        private static void ConfigureBuisnessLogic(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>();

            services.AddTransient<ManagementApi>();
            services.AddTransient<AuthenticationApi>();
            services.AddScoped<BuisnessLogicApi>();

            services.AddScoped<RepositoryFacade>();

            services.AddTransient<CreateRequestHandler>();
            services.AddTransient<GetRequestHandler>();
            services.AddTransient<UpdateRequestHandler>();
            services.AddTransient<DeleteRequestHandler>();
            services.AddTransient<UserHasPasswordRequestHandler>();

            services.AddTransient<AuthenticateRequestHandler>();

            services.AddTransient<JwtProvider>();
            services.AddTransient<JwtOptions>();
            services.AddTransient<PasswordHasher>();

            services.AddTransient<DbPasswordHashBuilder>();
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

            app.UseServiceAuthorization();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }
    }
}
