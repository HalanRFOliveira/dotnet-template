using Dotnet.Template.Infra.CrossCutting.Domain;

namespace Dotnet.Template.Domain.ActivityLogs
{
    public class ActivityLog : EntityBase<long>
    {
        public int? UserId { get; set; }


        public object ObjectRef { get; set; }

        public string Details { get; set; }

        public DateTime AddTime { get; set; }

    }
}
