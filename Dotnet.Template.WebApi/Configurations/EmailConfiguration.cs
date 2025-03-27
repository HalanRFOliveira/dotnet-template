using Dotnet.Template.Infra.Email;

namespace Dotnet.Template.WebApi.Configurations
{
	/// <summary>
	/// 
	/// </summary>
    public static class EmailConfiguration
	{
		/// <summary>
		/// Configura e injeta o serviço de e-mail
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		public static void AddEmailService(this IServiceCollection services, IConfiguration configuration)
		{
			services.Configure<EmailServiceOptions>(configuration.GetSection("EmailService"));
			services.AddScoped<IEmailService, EmailService>();
		}
	}
}
