using Dotnet.Template.Infra.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Dotnet.Template.Workers
{
    [DisallowConcurrentExecution]
    public class UsersIntegrationJob(
        ILogger<UsersIntegrationJob> logger,
        IEmailService emailService,
        IConfiguration configuration) : IJob
    {
        private readonly ILogger<UsersIntegrationJob> _logger = logger;
        private readonly IEmailService _emailService = emailService;
        private readonly IConfiguration _configuration = configuration;

        public async Task Execute(IJobExecutionContext context)
        {
            var applicationHost = _configuration.GetValue<string>("BaseUrl");
            _logger.LogInformation("");
            var alertTo = new List<string>();

            await _emailService.SendAsHtmlAsync(alertTo, "", "");

            _logger.LogInformation($"");
        }

    }
}
