namespace Dotnet.Templates.Domain.ActivityLogs
{
    public class GetActivityLogsFilter
	{
		public string Search { get; set; }

		public DateTime? PeriodStartAt { get; set; }

		public DateTime? PeriodEndAt { get; set; }

	}
}
