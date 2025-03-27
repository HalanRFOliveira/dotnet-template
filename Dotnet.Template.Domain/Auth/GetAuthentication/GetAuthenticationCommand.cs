using Dotnet.Template.Infra.Messaging;

namespace Dotnet.Template.Domain.Auth
{
    public class GetAuthenticationCommand : Command<GetAuthenticationCommandResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
