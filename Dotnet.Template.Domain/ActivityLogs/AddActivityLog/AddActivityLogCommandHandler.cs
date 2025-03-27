using Dotnet.Template.Infra.Messaging;

namespace Dotnet.Template.Domain.ActivityLogs
{
    public class AddActivityLogCommandHandler(
		IActivityLogRepository activityLogRepository
		) : CommandHandler<AddActivityLogCommand, IdentityResult<long>>()
	{
		private readonly IActivityLogRepository _activityLogRepository = activityLogRepository;

        public override async Task<CommandResponse<IdentityResult<long>>> Handle(AddActivityLogCommand request, CancellationToken cancellationToken)
		{
			if (!request.IsValid()) return Response(request.GetValidationResult());

            var log = new ActivityLog
			{
				UserId = request.UserId,
				Details = request.Details,
				ObjectRef = request.ObjectRef,
				AddDate = DateTime.Now,
			};
			await _activityLogRepository.AddAsync(log);

			return Response(new IdentityResult<long>(log.Id));
		}
	}
}
