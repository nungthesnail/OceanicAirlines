using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using InterserviceCommunication;
using System.Text;
using JwtUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Primitives;
using SharedFunctionality.Services.Caching;
using Microsoft.Extensions.Configuration;


namespace SharedFunctionality.AspNetCore
{
    /// <summary>
    /// Конфигуратор общих для всех микросервисов сервисов для внедрения зависимостей
    /// и компонентов middlewares
    /// </summary>
    public static class StartupConfigurator
    {
        /// <summary>
        /// Добавляет кастомную настройку Swagger Gen с поддержкой авторизации
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        public static void AddSwaggerGenWithAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "token",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                       new OpenApiSecurityScheme
                       {
                           Reference = new OpenApiReference
                           {
                               Type = ReferenceType.SecurityScheme,
                               Id = "Bearer"
                           }
                       },
                       new string[] { }
                   }
                });
            });
        }

        /// <summary>
        /// Добавляет межсервисного связиста в коллекцию сервисов
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="config">Конфигурация приложения</param>
        /// <returns></returns>
        public static async Task AddInterserviceCommunicator(this IServiceCollection services, IConfiguration config) // Guid serviceId, string servicePassword)
        {
            var serviceId = GetServiceId(config);
            var servicePassword = GetServicePassword(config);

            var communicatorSettings = new InterserviceCommunicatorSettings()
            {
                ServiceId = serviceId,
                ServicePassword = servicePassword,
                DoAuthentication = true,
                AuthenticateImmediately = false
            };

            var communicator = await InterserviceCommunicator.CreateInterserviceCommunicator(communicatorSettings, config);

            services.AddSingleton(communicator);
        }

        private static Guid GetServiceId(IConfiguration config)
        {
            var serviceIdString = config["ServiceSettings:ServiceId"];

            try
            {
                var result = Guid.Parse(serviceIdString ?? "");
                return result;
            }
            catch (FormatException)
            {
                return Guid.Empty;
            }
        }

        private static string GetServicePassword(IConfiguration config)
        {
            return config["ServiceSettings:ServicePassword"] ?? String.Empty;

		}

        /// <summary>
        /// Конфигурирует настройки аутентификации
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="config">Конфигурация приложения</param>
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = "AuthenticationService",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(new JwtOptions(config).Secret))
                    };

                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            var headers = context.Request.Headers;
                            var cookies = context.Request.Cookies;

                            var authHeader = headers["token"];
                            var authCookie = cookies["token"];

                            if (authHeader != StringValues.Empty)
                            {
                                context.Token = authHeader;
                            }
                            else if (authCookie != null)
                            {
                                context.Token = authCookie;
                            }
                            else
                            {
                                context.Token = null;
                            }

                            return Task.CompletedTask;
                        }
                    };
                }
            );
        }

        /// <summary>
        /// Конфигурирует авторизацию
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization();
        }

        /// <summary>
        /// Конфигурирует кэширование Redis
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="config">Конфигурация приложения</param>
        /// <param name="instanceName">Название экземпляра кэширования</param>
        public static void ConfigureCaching(this IServiceCollection services, IConfiguration config, string? instanceName = null)
        {
			services.AddStackExchangeRedisCache(options =>
			{
				options.Configuration = config["ServiceSettings:RedisAddress"];
				options.InstanceName = instanceName;
			});

            services.AddTransient<ICachingService, CachingService>();
		}

        /// <summary>
        /// Конфигурирует компонент middleware веб-приложения, необходимый для выполнения авторизации
        /// межсервисного связиста при первом запросе к веб-приложению
        /// </summary>
        /// <param name="app">Объект веб-приложения</param>
        public static void UseServiceAuthorization(this WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                InterserviceCommunicator communicator
                    = context.RequestServices.GetService<InterserviceCommunicator>()!;

                if (!communicator.IsAuthenticated())
                {
                    await communicator.RequestAuthorization();
                }

                await next.Invoke();
            });
        }
    }
}
