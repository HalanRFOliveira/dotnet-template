namespace Dotnet.Templates.Domain.ActivityLogs
{
    public class GetActivityLogsCommandResult
	{
		public long Id { get; set; }
		public string Status { get; set; }
		public string TypeName { get; set; }
		public string Details { get; set; }
		public string ObjectRef { get; set; }
		public int? UserId { get; set; }
		public string UserName { get; set; }
		public DateTime AddTime { get; set; }
	}
}