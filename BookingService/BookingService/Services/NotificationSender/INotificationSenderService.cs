namespace BookingService.Services.NotificationSender
{
	/// <summary>
	/// Интерфейс отправителя уведомлений
	/// </summary>
	public interface INotificationSenderService
	{
		/// <summary>
		/// Отправляет уведомление
		/// </summary>
		/// <param name="notification">Описание уведомления</param>
		public void Send(Notification notification);
	}
}