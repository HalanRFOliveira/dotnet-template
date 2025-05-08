using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Dotnet.Template.Infra.Email
{
    /// <summary>
    /// Responsável pelo envio de e-mails
    /// </summary>
    public class EmailService(IOptions<EmailServiceOptions> options) : IEmailService
	{
		private readonly EmailServiceOptions _options = options.Value;


        /// <summary>
        /// Envia e-mail com MailKit
        /// </summary>
        /// <param name="to">Destinatário</param>
        /// <param name="subject">Assunto</param>
        /// <param name="message">Corpo do e-mail</param>
        /// <param name="attachments">Anexos (FileName / Byte[])</param>
        public void Send(IEnumerable<string> to, string subject, string message, Dictionary<string, byte[]> attachments = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null)
		{
			Send(to, subject, message, false, attachments, cc: cc, bcc: bcc);
		}


		/// <summary>
		/// Envia e-mail como HTML com MailKit
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		public void SendAsHtml(IEnumerable<string> to, string subject, string message, Dictionary<string, byte[]> attachments = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null)
		{
			Send(to, subject, message, true, attachments, cc: cc, bcc: bcc);
		}


		/// <summary>
		/// Envia e-mail com MailKit, Assincrono
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		public Task SendAsync(IEnumerable<string> to, string subject, string message, Dictionary<string, byte[]> attachments = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null)
		{
			return SendAsync(to, subject, message, false, attachments, cc: cc, bcc: bcc);
		}


		/// <summary>
		/// Envia e-mail como HTML com MailKit, Assincrono
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		public Task SendAsHtmlAsync(IEnumerable<string> to, string subject, string message, Dictionary<string, byte[]> attachments = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null)
		{
			return SendAsync(to, subject, message, true, attachments, cc: cc, bcc: bcc);
		}


		/// <summary>
		/// Envia e-mail com MailKit, Assincrono
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		/// <param name="sender">Informações do Remetente</param>
		public Task SendAsync(IEnumerable<string> to, string subject, string message, bool isHtml = false, Dictionary<string, byte[]> attachments = null, EmailInfo sender = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null)
		{
			return Task.Factory.StartNew(() =>
			{
				Send(to, subject, message, isHtml, attachments, sender, cc, bcc);
			});
		}


		/// <summary>
		/// Envia e-mail com MailKit
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		/// <param name="sender">Informações do Remetente</param>
		public void Send(IEnumerable<string> to, string subject, string message, bool isHtml = false, Dictionary<string, byte[]> attachments = null, EmailInfo sender = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null)
		{
			// create message
			var mimeMessage = new MimeMessage();
			//add mail from
			mimeMessage.From.Add(
				string.IsNullOrWhiteSpace(sender?.FromEmail) || string.IsNullOrWhiteSpace(sender?.FromName)
					? new MailboxAddress(_options.DefaultFromName, _options.DefaultFromEmail)
					: new MailboxAddress(sender.FromName, sender.FromEmail)
				);
			mimeMessage.To.AddRange(to.Select(p => MailboxAddress.Parse(p)));
			if (cc != null) mimeMessage.Cc.AddRange(cc.Select(p => MailboxAddress.Parse(p)));
			if (bcc != null) mimeMessage.Bcc.AddRange(bcc.Select(p => MailboxAddress.Parse(p)));
			mimeMessage.Subject = subject;

			// BodyBuilder
			var builder = isHtml
				? new BodyBuilder { HtmlBody = message }
				: new BodyBuilder { TextBody = message };

			//	Handle Attachments
			if (attachments != null)
			{
				foreach (var item in attachments)
				{
					builder.Attachments.Add(item.Key, item.Value);
				}
			}

			mimeMessage.Body = builder.ToMessageBody();

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_options.SmtpHost, _options.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.SmtpUser, _options.SmtpPass);
            smtp.Send(mimeMessage);
            smtp.Disconnect(true);
        }
	}

	public class EmailInfo
	{
		public string FromEmail { get; set; }

		public string FromName { get; set; }
	}
}
