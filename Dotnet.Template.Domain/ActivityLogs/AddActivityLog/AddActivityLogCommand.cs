using Dotnet.Template.Infra.Messaging;

namespace Dotnet.Template.Domain.ActivityLogs
{
    public class AddActivityLogCommand : Command<IdentityResult<long>>
	{
		public int? UserId { get; set; }

		public ActivityLogType TypeId { get; set; }

		public int ObjectRef { get; set; }

		public string Details { get; set; }
	}
}