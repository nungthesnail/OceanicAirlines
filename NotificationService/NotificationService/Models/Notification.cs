namespace NotificationService.Models
{
	public class Notification
	{
		public enum ReceiversTypes
		{
			RECEIVER_EMAIL = 0,
			RECEIVER_USERID = 1
		}

		public int ReceiverType { get; set; }

		public string ReceiverData { get; set; } = null!;

		public string Message { get; set; } = null!;
	}
}
