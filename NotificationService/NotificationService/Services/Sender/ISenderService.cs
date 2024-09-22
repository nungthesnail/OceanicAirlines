using NotificationService.Models;

namespace NotificationService.Services.SenderService
{
	public interface ISenderService
	{
		public Task Send(Notification notification);
	}
}