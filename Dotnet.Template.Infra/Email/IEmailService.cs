namespace Dotnet.Template.Infra.Email
{
    /// <summary>
    /// Responsável pelo envio de e-mails
    /// </summary>
    public interface IEmailService
	{
		/// <summary>
		/// Envia e-mail
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		void Send(IEnumerable<string> to, string subject, string message, Dictionary<string, byte[]> attachments = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null);


		/// <summary>
		/// Envia e-mail como HTML
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		void SendAsHtml(IEnumerable<string> to, string subject, string message, Dictionary<string, byte[]> attachments = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null);


		/// <summary>
		/// Envia e-mail, Assincrono
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		Task SendAsync(IEnumerable<string> to, string subject, string message, Dictionary<string, byte[]> attachments = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null);

		/// <summary>
		/// Envia e-mail como HTML, Assincrono
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		Task SendAsHtmlAsync(IEnumerable<string> to, string subject, string message, Dictionary<string, byte[]> attachments = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null);


		/// <summary>
		/// Envia e-mail, Assincrono
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		/// <param name="sender">Informações do Remetente</param>
		Task SendAsync(IEnumerable<string> to, string subject, string message, bool isHtml = false, Dictionary<string, byte[]> attachments = null, EmailInfo sender = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null);


		/// <summary>
		/// Envia e-mail
		/// </summary>
		/// <param name="to">Destinatário</param>
		/// <param name="subject">Assunto</param>
		/// <param name="message">Corpo do e-mail</param>
		/// <param name="attachments">Anexos (FileName / Byte[])</param>
		/// <param name="sender">Informações do Remetente</param>
		void Send(IEnumerable<string> to, string subject, string message, bool isHtml = false, Dictionary<string, byte[]> attachments = null, EmailInfo sender = null, IEnumerable<string> cc = null, IEnumerable<string> bcc = null);
	}
}
