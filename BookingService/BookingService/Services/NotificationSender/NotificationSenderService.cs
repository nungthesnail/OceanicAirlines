using BookingService.Services.RabbitMq;


namespace BookingService.Services.NotificationSender
{
	/// <summary>
	/// Сервис отправителя уведомлений
	/// </summary>
	public class NotificationSenderService : INotificationSenderService
	{
		private IRabbitMqService _rabbitMq = null!;

		/// <summary>
		/// Конструктор для внедрения зависимостей
		/// </summary>
		/// <param name="rabbitMq">Сервис брокера сообщений</param>
		public NotificationSenderService(IRabbitMqService rabbitMq)
		{
			_rabbitMq = rabbitMq;
		}

		public void Send(Notification notification)
		{
			_rabbitMq.Send(notification);
		}
	}
}
