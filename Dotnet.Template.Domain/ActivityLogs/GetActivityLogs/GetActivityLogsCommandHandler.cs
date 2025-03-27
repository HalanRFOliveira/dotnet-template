using Dotnet.Template.Infra.Messaging;
using Dotnet.Template.Infra.Paging;

namespace Dotnet.Template.Domain.ActivityLogs
{
    public class GetActivityLogsCommandHandler : CommandHandler<GetActivityLogsCommand, PagedResult<GetActivityLogsCommandResult>>
	{
		private readonly IActivityLogRepository _logsRepository;

		public GetActivityLogsCommandHandler(
			IActivityLogRepository dealRepository
			) : base()
		{
			_logsRepository = dealRepository;
		}

		public override async Task<CommandResponse<PagedResult<GetActivityLogsCommandResult>>> Handle(GetActivityLogsCommand request, CancellationToken cancellationToken)
		{
			if (!request.IsValid()) return Response(request.GetValidationResult());

			var result = await _logsRepository.GetActivityLogsAsync(request.Filter);

			return Response(result);
		}
	}
}
