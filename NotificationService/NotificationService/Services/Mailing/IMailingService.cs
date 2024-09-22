
namespace NotificationService.Services.Mailing
{
	public interface IMailingService
	{
		public Task Send(MailMessage message);
	}
}