using Dotnet.Template.Infra.CrossCutting.Repositories;
using Dotnet.Template.Infra.Paging;

namespace Dotnet.Template.Domain.ActivityLogs
{
    public interface IActivityLogRepository : IRepository
	{
        Task AddAsync(ActivityLog log);
        Task<PagedResult<GetActivityLogsCommandResult>> GetActivityLogsAsync(Filter<PagedFilter<ActivityLogType>> filter);
	}
}
