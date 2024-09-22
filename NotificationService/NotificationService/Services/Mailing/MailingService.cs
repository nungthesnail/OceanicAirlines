using MailKit.Net.Smtp;
using MimeKit;


namespace NotificationService.Services.Mailing
{
	public class MailingService : IMailingService
	{
		private readonly ILogger<MailingService> _logger;

		private readonly IConfiguration _config;

        public MailingService(ILogger<MailingService> logger, IConfiguration config)
        {
            _logger = logger;
			_config = config;
        }

        public async Task Send(MailMessage message)
		{
			using var mimeMessage = BuildMessage(message);
			using (var client = new SmtpClient())
			{
				var (smtpServer, smtpPort) = (
					_config["ServiceSettings:SmtpServerAddress"],
					Int32.Parse(_config["ServiceSettings:SmtpServerPort"] ?? "465")
				);

				var (smtpUsername, smtpPassword) = (
					_config["ServiceSettings:SmtpServerUsername"],
					_config["ServiceSettings:SmtpServerPassword"]
				);

				await client.ConnectAsync(smtpServer, smtpPort, true);
				await client.AuthenticateAsync(smtpUsername, smtpPassword);
				await client.SendAsync(mimeMessage);

				await client.DisconnectAsync(true);
			}
		}

		private MimeMessage BuildMessage(MailMessage source)
		{
			var (smtpUsername, senderName) = (
				_config["ServiceSettings:SmtpServerUsername"],
				_config["ServiceSettings:MailSenderName"]
			);

			var mimeMessage = new MimeMessage();

			mimeMessage.From.Add(new MailboxAddress(senderName, smtpUsername));
			mimeMessage.To.Add(new MailboxAddress("", source.ReceiverMailbox));
			mimeMessage.Subject = senderName;
			mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
			{
				Text = source.Message
			};

			return mimeMessage;
		}
	}
}
