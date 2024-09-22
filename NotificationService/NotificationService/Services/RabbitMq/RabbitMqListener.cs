using NotificationService.Models;
using NotificationService.Services.SenderService;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;


namespace NotificationService.Services.RabbitMq
{
	public class RabbitMqListener : BackgroundService
	{
        private readonly ISenderService _sender;
		private readonly ILogger<RabbitMqListener> _logger;
		private readonly IConfiguration _config;

		private IConnection _connection = null!;
		private IModel _channel = null!;

        public RabbitMqListener(
			ISenderService sender,
			ILogger<RabbitMqListener> logger,
			IConfiguration config)
        {
            _sender = sender;
			_logger = logger;
			_config = config;

			InitializeRabbitMq();
        }

        private void InitializeRabbitMq()
        {
			var connFactory = new ConnectionFactory()
			{
				HostName = _config["ServiceSettings:RabbitMqAddress"],
				Port = Int32.Parse(_config["ServiceSettings:RabbitMqPort"] ?? "5672")
			};

			_connection = connFactory.CreateConnection();
			_channel = _connection.CreateModel();

			_channel.QueueDeclare(
				queue: "NotificationQueue",
				durable: false,
				exclusive: false,
				autoDelete: false,
				arguments: null
			);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

			consumer.Received += Receive;

            _channel.BasicConsume("NotificationQueue", false, consumer);

            return Task.CompletedTask;
		}

		private void Receive(object? ch, BasicDeliverEventArgs ea)
		{
			var content = Encoding.UTF8.GetString(ea.Body.ToArray());

			SendNotification(content);

			_channel.BasicAck(ea.DeliveryTag, false);
		}

		public override void Dispose()
		{
            _channel.Close();
            _connection.Close();

            base.Dispose();
		}

		private void SendNotification(string content)
		{
			try
			{
				var notification = JsonSerializer.Deserialize<Notification>(content);

				_sender.Send(notification!);
			}
			catch (JsonException)
			{
				// TODO
			}
		}
	}
}
