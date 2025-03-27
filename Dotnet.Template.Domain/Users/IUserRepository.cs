using Dotnet.Template.Infra.CrossCutting.Repositories;
using Dotnet.Template.Infra.Paging;

namespace Dotnet.Template.Domain.Users
{
    public interface IUserRepository : IRepository
    {
        PagedResult<GetUsersCommandResult> GetUsers(Filter<PagedFilter<string>> filter);
        Task<GetUsersCommandResult> GetUserAsync(int id);
        User FindByEmail(string email);
        Task<User> FindByIdAsync(int id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        User GetUserFromNameOrEmail(string name, string email);
        User GetLoggedUser();
        IEnumerable<GetUsersCommandResult> SearchUsersByEmailOrName(string term);
    }
}
