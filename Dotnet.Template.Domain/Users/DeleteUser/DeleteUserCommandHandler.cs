using Dotnet.Template.Domain.ActivityLogs;
using Dotnet.Template.Infra.Messaging;

namespace Dotnet.Template.Domain.Users
{
    public class DeleteUserCommandHandler(
        IUserRepository userRepository,
        ActivityLogHelper activityLogHelper
            ) : CommandHandler<DeleteUserCommand, IdentityResult<int>>()
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ActivityLogHelper _activityLogHelper = activityLogHelper;

        public override async Task<CommandResponse<IdentityResult<int>>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsValid()) return Response(request.GetValidationResult());

            await _userRepository.DeleteUserAsync(request.Id);

            await _activityLogHelper.LogAsync(ActivityLogType.DeleteUser, request.Id);

            return Response(new IdentityResult<int>(request.Id));
        }
    }
}
