using Dotnet.Template.Infra.Paging;

namespace Dotnet.Template.Domain.Users
{
    public class GetUsersCommand : CommandWithFilter<PagedFilter<string>, GetUsersCommandResult> { }
}
