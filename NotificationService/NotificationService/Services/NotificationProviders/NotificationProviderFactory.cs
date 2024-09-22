using NotificationService.Models;

namespace NotificationService.Services.NotificationProviders
{
	public class NotificationProviderFactory
	{
        private readonly IServiceProvider _services = null!;

        public NotificationProviderFactory(IServiceProvider services)
        {
            _services = services;
        }

        public INotificationProvider CreateByReceiverType(int receiverType)
        {
            try
            {
                switch ((Notification.ReceiversTypes)receiverType)
                {
                    case Notification.ReceiversTypes.RECEIVER_EMAIL:
                        return CreateEmailNotificationProvider();

                    case Notification.ReceiversTypes.RECEIVER_USERID:
                        return CreateUserIdNotificationProvider();

                    default:
                        throw new InvalidOperationException($"Receiver type is invalid {receiverType}");
                }
            }
            catch (InvalidCastException)
            {
                throw new InvalidOperationException($"Receiver type is invalid {receiverType}");
            }
        }

        public INotificationProvider CreateUserIdNotificationProvider()
        {
            INotificationProvider provider = _services.GetRequiredService<UserIdNotificationProvider>();

            return provider;
        }

		public INotificationProvider CreateEmailNotificationProvider()
		{
			INotificationProvider provider = _services.GetRequiredService<EmailNotificationProvider>();

			return provider;
		}
	}
}
