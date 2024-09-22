namespace NotificationService.Services.Mailing
{
	public class MailMessage
	{
		public string ReceiverMailbox { get; set; } = null!;

		public string Message { get; set; } = null!;
	}
}
