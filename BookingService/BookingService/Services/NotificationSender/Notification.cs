namespace BookingService.Services.NotificationSender
{
	/// <summary>
	/// Описание уведомления
	/// </summary>
	public class Notification
	{
		/// <summary>
		/// Тип способа получения уведомления:
		/// 0 - получение по электронной почте
		/// 1 - тип получения автоматически решается для пользователя с указанным уникальным идентификатором
		/// </summary>
		public enum ReceiversTypes
		{
			RECEIVER_EMAIL = 0,
			RECEIVER_USERID = 1
		}

		/// <summary>
		/// Тип получения уведомления
		/// </summary>
		public ReceiversTypes ReceiverType { get; set; }

		/// <summary>
		/// Данные, необходимые для адресации уведомления
		/// </summary>
		public string ReceiverData { get; set; } = null!;

		/// <summary>
		/// Сообщение уведомления
		/// </summary>
		public string Message { get; set; } = null!;
	}
}
