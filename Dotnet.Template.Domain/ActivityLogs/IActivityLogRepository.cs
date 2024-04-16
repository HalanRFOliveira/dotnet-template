using Dotnet.Template.Infra.CrossCutting.Repository;
using Dotnet.Template.Infra.Paging;

namespace Dotnet.Templates.Domain.ActivityLogs
{
    public interface IActivityLogRepository : IRepository
	{
		Task<PagedResult<GetActivityLogsCommandResult>> GetActivityLogsAsync(Filter<GetActivityLogsFilter> filter);
	}
}
