using NotificationService.Models;

namespace NotificationService.Services.NotificationProviders
{
	public interface INotificationProvider
	{
		public Task Send(Notification notification);
	}
}