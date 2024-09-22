using NotificationService.Services.NotificationProviders;
using NotificationService.Models;

namespace NotificationService.Services.SenderService
{
	public class SenderService : ISenderService
	{
		private readonly NotificationProviderFactory _providerFactory = null!;

		public SenderService(NotificationProviderFactory providerFactory)
		{
			_providerFactory = providerFactory;
		}

		public async Task Send(Notification notification)
		{
			var provider = _providerFactory.CreateByReceiverType(notification.ReceiverType);
			await provider.Send(notification);
		}
	}
}
