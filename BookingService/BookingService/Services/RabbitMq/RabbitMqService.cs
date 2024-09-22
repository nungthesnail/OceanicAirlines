using RabbitMQ.Client;
using System.Text;
using System.Text.Json;


namespace BookingService.Services.RabbitMq
{
    /// <summary>
    /// Реализация сервиса взаимодействия с брокером сообщений RabbitMq
    /// </summary>
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Конструктор для внедрения зависимостей
        /// </summary>
        /// <param name="config">Конфигурация приложения</param>
        public RabbitMqService(IConfiguration config)
        {
            _config = config;
        }

        public void Send(string message)
        {
            var connFactory = new ConnectionFactory()
            {
                HostName = _config["ServiceSettings:RabbitMqAddress"],
                Port = Int32.Parse(_config["ServiceSettings:RabbitMqPort"] ?? "5672")
			};

            using (var conn = connFactory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "NotificationQueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "NotificationQueue",
                    basicProperties: null,
                    body: body
                );
            }
        }

        public void Send(object? message)
        {
            var serialized = JsonSerializer.Serialize(message);
            Send(serialized);
        }
    }
}
