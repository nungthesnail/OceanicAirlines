using BuisnessLogic.Api;
using BuisnessLogic.Handlers;
using BuisnessLogic.Repository;
using EntityFrameworkLogic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BuisnessLogic
{
    /// <summary>
    /// Строитель API для взаимодействия с бизнес-логикой сервиса пользователей
    /// </summary>
    public class BuisnessLogicApiBuilder
    {
        private IServiceCollection _services = null!;

        private IConfiguration _configuration = null!;

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public BuisnessLogicApiBuilder()
        {
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            _services = new ServiceCollection();

            ConfigureRepository();
            ConfigureRequestHandlers();
        }

        private void ConfigureRepository()
        {
            _services.AddDbContext<ApplicationContext>();

            _services.AddTransient<RepositoryFacade>();

        }

        private void ConfigureRequestHandlers()
        {
            _services.AddTransient<CreateRequestHandler>();
            _services.AddTransient<GetRequestHandler>();
            _services.AddTransient<UpdateRequestHandler>();
            _services.AddTransient<DeleteRequestHandler>();
            
            _services.AddTransient<UserExistsRequestHandler>();
        }

        /// <summary>
        /// Установка конфигурации приложения
        /// </summary>
        /// <param name="config">Конфигурация</param>
        public void SetConfiguration(IConfiguration config)
        {
            _configuration = config;
        }

        /// <summary>
        /// Метод построения API бизнес-логики сервиса пользователей
        /// </summary>
        /// <returns>Построенное API бизнес-логики сервиса пользователей</returns>
        public BuisnessLogicApi Build()
        {
            AddConfigurationToServices();

            var serviceProvider = _services.BuildServiceProvider();

            return new BuisnessLogicApi(serviceProvider);
        }

        private void AddConfigurationToServices()
        {
            ThrowIfConfigurationIsntSpecified();

            _services.AddSingleton(_configuration);
        }

        private void ThrowIfConfigurationIsntSpecified()
        {
			if (_configuration == null)
			{
				throw new InvalidOperationException("Configuration isn\'t specified");
			}
		}
    }
}
