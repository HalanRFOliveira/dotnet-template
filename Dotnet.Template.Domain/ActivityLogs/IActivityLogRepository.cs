using Dotnet.Template.Infra.CrossCutting.Repository;
using Dotnet.Template.Infra.Paging;

namespace Dotnet.Template.Domain.ActivityLogs
{
    public interface IActivityLogRepository : IRepository
	{
		Task<PagedResult<GetActivityLogsCommandResult>> GetActivityLogsAsync(Filter<PagedFilter<ActivityLogType>> filter);
	}
}
