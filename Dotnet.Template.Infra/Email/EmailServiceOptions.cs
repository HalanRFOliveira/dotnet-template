namespace Dotnet.Template.Infra.Email
{
	public class EmailServiceOptions
	{
		public string SmtpHost { get; set; }

		public int SmtpPort { get; set; }

		public string SmtpUser { get; set; }

		public string SmtpPass { get; set; }

		public string DefaultFromEmail { get; set; }

		public string DefaultFromName { get; set; }
	}
}
