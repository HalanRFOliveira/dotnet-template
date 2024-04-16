﻿using Dotnet.Template.Infra.Paging;
using Dotnet.Templates.Domain.ActivityLogs;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Template.Data.Repository
{
    public class ActivityLogRepository : RepositoryBase, IActivityLogRepository
    {
        public ActivityLogRepository(MySqlContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<GetActivityLogsCommandResult>> GetActivityLogsAsync(Filter<GetActivityLogsFilter> filter)
        {

            var startAt = filter.Data?.PeriodStartAt ?? DateTime.Now.AddDays(-30);
            var endAt = (filter.Data?.PeriodEndAt ?? DateTime.Now).Date.AddSeconds(-1).AddDays(1);
            var query = _dbContext.ActivityLogs.Where(p => startAt <= p.AddTime && p.AddTime <= endAt);
            if (!string.IsNullOrWhiteSpace(filter.Data?.Search))
            {
                query = query.Where(p => p.Details.Contains(filter.Data.Search) || (p.ObjectRef as string).Contains(filter.Data.Search));
            }

            var totalSize = query.Count();

            if (filter.HasSorter)
            {
                var descending = filter.Descending.Value;
                query = filter.SortBy switch
                {
                    ("addTime") => descending ? query.OrderByDescending(p => p.AddTime) : query.OrderBy(p => p.AddTime),
                    _ => query.OrderByDescending(p => p.Id),
                };
            }

            var pagedData = await query
                .Skip(filter.SkipCount)
                .Take(filter.Limit)
                .Select(p => new GetActivityLogsCommandResult
                {
                    Id = p.Id,
                    Details = p.Details,
                    ObjectRef = p.ObjectRef != null ? p.ObjectRef.ToString() : string.Empty,
                    UserId = p.UserId,
                    AddTime = p.AddTime
                })
                .ToListAsync();

            return new PagedResult<GetActivityLogsCommandResult>(totalSize, filter.Limit, pagedData);

        }
    }
}
