using Dotnet.Template.Infra.Messaging;

namespace Dotnet.Template.Domain.Users
{
    public class SearchUsersByEmailOrNameCommand : Command<IEnumerable<GetUsersCommandResult>>
    {
        public string Term { get; set; }
    }
}
