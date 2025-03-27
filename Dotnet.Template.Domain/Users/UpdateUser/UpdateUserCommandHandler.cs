using Dotnet.Template.Domain.ActivityLogs;
using Dotnet.Template.Infra.Messaging;

namespace Dotnet.Template.Domain.Users
{
    public class UpdateUserCommandHandler(
        IUserRepository userRepository,
            ActivityLogHelper activityLogHelper
            ) : CommandHandler<UpdateUserCommand, IdentityResult<int>>()
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ActivityLogHelper _activityLogHelper = activityLogHelper;

        public override async Task<CommandResponse<IdentityResult<int>>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return Response(request.GetValidationResult());

            var user = await _userRepository.FindByIdAsync(request.Id);

            user.Name = request.Name;
            user.Email = request.Email;
            user.UpdateDate = DateTime.Now;
            user.Access = String.Join(";", request.Access);

            var userAlreadyExistsResult = new UniqueUserEmailSpec(_userRepository).Validate(user);
            if (!userAlreadyExistsResult.IsValid) return Response(userAlreadyExistsResult);

            await _userRepository.UpdateUserAsync(user);

            await _activityLogHelper.LogAsync(ActivityLogType.UpdateUser, user.Id);

            return Response(new IdentityResult<int>(user.Id));
        }
    }
}
