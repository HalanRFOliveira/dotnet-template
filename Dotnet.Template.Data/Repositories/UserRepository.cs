using Dotnet.Template.Domain.Users;
using Dotnet.Template.Infra.HttpContext;
using Dotnet.Template.Infra.Paging;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Template.Data.Repositories
{
    public class UserRepository(MySqlContext dbContext, IHttpUserContext httpUserContext) : RepositoryBase(dbContext), IUserRepository
    {
        private readonly MySqlContext _dbContext = dbContext;
        private readonly IHttpUserContext _httpUserContext = httpUserContext;

        public PagedResult<GetUsersCommandResult> GetUsers(Filter<PagedFilter<string>> filter)
        {

            var startAt = filter.Data?.PeriodStartAt ?? DateTime.Now.AddYears(-1);
            var endAt = filter.Data?.PeriodEndAt ?? DateTime.Now;
            endAt = new DateTime(endAt.Year, endAt.Month, endAt.Day, 23, 59, 59);
            var query = _dbContext.Users
                .Where(p => startAt <= p.AddDate && p.AddDate <= endAt);

            if (!string.IsNullOrWhiteSpace(filter.Data.Search))
            {
                if (int.TryParse(filter.Data?.Search, out int userId))
                {
                    query = query.Where(p => p.Id == userId);
                }
                else
                {
                    query = query.Where(p =>
                           p.Name.Contains(filter.Data.Search)
                           || p.Email.Contains(filter.Data.Search)
                           || (p.Name != null && p.Name.Contains(filter.Data.Search)));
                }
            }

            var totalSize = query.Count();

            if (filter.HasSorter)
            {
                var descending = filter.Descending.Value;
                query = filter.SortBy switch
                {
                    ("name") => descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                    ("date") => descending ? query.OrderByDescending(p => p.AddDate) : query.OrderBy(p => p.AddDate),
                    _ => descending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
                };
            }
            var pagedData = query
                .Skip(filter.SkipCount)
                .Take(filter.RowsPerPage)
                .Select(p => new GetUsersCommandResult
                {
                    Id = p.Id,
                    Name = p.Name,
                    AddDate = p.AddDate,
                    UpdateDate = p.UpdateDate,
                    Email = p.Email,
                    Access = p.Access.Split(),
                    Type = p.Type,
                });

            return new PagedResult<GetUsersCommandResult>(totalSize, pagedData);
        }


        public async Task AddUserAsync(User user)
        {
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public User FindByEmail(string email)
        {
            return _dbContext.Users
                .AsNoTracking()
                .IgnoreQueryFilters()
                .FirstOrDefault(x => x.Email.ToLower().Equals(email.ToLower()));
        }

        public User GetLoggedUser()
        {
            return _dbContext.Users
                .AsNoTracking()
                .IgnoreQueryFilters()
                .FirstOrDefault(x => x.Email.ToLower().Equals(_httpUserContext.TryGetEmailFromLoggedUser().ToLower()));
        }

        public async Task<User> FindByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(f => f.Id == id);
        }

        public User GetUserFromNameOrEmail(string name, string email)
        {
            return _dbContext.Users.FirstOrDefault(f => f.Name == name || f.Email == email);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await FindByIdAsync(id);
            _dbContext.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<GetUsersCommandResult> GetUserAsync(int id)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) return default;

            return new GetUsersCommandResult
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                AddDate = user.AddDate,
                UpdateDate = user.UpdateDate,
                Access = user.Access.Split(";"),
            };
        }

        public IEnumerable<GetUsersCommandResult> SearchUsersByEmailOrName(string term)
        {
            return _dbContext.Users
                .AsNoTracking()
                .Where(x => x.Name.Contains(term) || x.Email.Contains(term))
                .Select(user => new GetUsersCommandResult
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    AddDate = user.AddDate,
                    UpdateDate = user.UpdateDate,
                    Access = user.Access.Split(),
                });
        }
    }
}
