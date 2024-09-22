using NotificationService.Models;
using NotificationService.Services.Mailing;

namespace NotificationService.Services.NotificationProviders
{
	public class EmailNotificationProvider : INotificationProvider
	{
		private readonly IMailingService _mailing = null!;

		private readonly MailMessageBuilder _mailMessageBuilder = null!;

        public EmailNotificationProvider(
			IMailingService mailing,
			MailMessageBuilder mailMessageBuilder
		)
		{
            _mailing = mailing;
			_mailMessageBuilder = mailMessageBuilder;
        }

		public async Task Send(Notification notification)
		{
			var message = BuildMessage(notification);
			await _mailing.Send(message);
		}

		private MailMessage BuildMessage(Notification notification)
		{
			_mailMessageBuilder.SetReceiverMailbox(notification.ReceiverData);
			_mailMessageBuilder.SetMessage(notification.Message);

			return _mailMessageBuilder.Build();
		}
    }
}
