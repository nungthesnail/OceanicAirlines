namespace NotificationService.Services.Mailing
{
	public class MailMessageBuilder
	{
		private string? _receiverMailbox = null;

		private string? _message = null;

		public void SetReceiverMailbox(string mailbox)
		{
			_receiverMailbox = mailbox;
		}

		public void SetMessage(string message)
		{
			_message = message;
		}

		public MailMessage Build()
		{
			ThrowIfBuildingIncompleted();

			var result = new MailMessage
			{
				ReceiverMailbox = _receiverMailbox!,
				Message = _message!
			};

			return result;
		}

		private void ThrowIfBuildingIncompleted()
		{
			var completed = _receiverMailbox != null && _message != null;

			if (!completed)
			{
				throw new InvalidOperationException();
			}
		}
	}
}
