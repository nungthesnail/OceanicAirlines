using InterserviceCommunication;
using InterserviceCommunication.Models.UserService;
using NotificationService.Models;
using NotificationService.Services.Mailing;


namespace NotificationService.Services.NotificationProviders
{
	public class UserIdNotificationProvider : INotificationProvider
	{
		private readonly IMailingService _mailing = null!;

		private readonly MailMessageBuilder _mailMessageBuilder = null!;

		private readonly InterserviceCommunicator _interserviceCommunicator = null!;

		public UserIdNotificationProvider(
			IMailingService mailing,
			MailMessageBuilder mailMessageBuilder,
			InterserviceCommunicator interserviceCommunicator
		)
		{
			_mailing = mailing;
			_mailMessageBuilder = mailMessageBuilder;
			_interserviceCommunicator = interserviceCommunicator;
		}

		public async Task Send(Notification notification)
		{
			var userId = GetUserId(notification);
			var userMail = await GetUserEmail(userId);

			var message = BuildMessage(userMail, notification.Message);

			await _mailing.Send(message);
		}

		private async Task<string> GetUserEmail(Guid userId)
		{
			var user = await GetUser(userId);

			return user.Email ?? "";
		}

		private Guid GetUserId(Notification notification)
		{
			try
			{
				return Guid.Parse(notification.ReceiverData);
			}
			catch (FormatException)
			{
				throw new InvalidOperationException($"Invalid user id format: {notification.ReceiverData}");
			}
		}

		private async Task<UserServiceUserModel> GetUser(Guid userId)
		{
			var connector = _interserviceCommunicator.GetUserServiceConnector();
			var request = connector.CreateGetUserRequest(userId);
			var result = await request.Send();

			return result;
		}

		private MailMessage BuildMessage(string mailbox, string message)
		{
			_mailMessageBuilder.SetReceiverMailbox(mailbox);
			_mailMessageBuilder.SetMessage(message);

			return _mailMessageBuilder.Build();
		}
	}
}
