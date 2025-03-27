using Dotnet.Template.Domain.ActivityLogs;
using Dotnet.Template.Infra.Messaging;
using Dotnet.Template.Infra.PasswordService;

namespace Dotnet.Template.Domain.Users
{
    public class AddUserCommandHandler(
        IUserRepository userRepository,
        IPasswordService passwordService,
        ActivityLogHelper activityLogHelper
            ) : CommandHandler<AddUserCommand, IdentityResult<int>>()
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ActivityLogHelper _activityLogHelper = activityLogHelper;

        public override async Task<CommandResponse<IdentityResult<int>>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return Response(request.GetValidationResult());

            //if (!request.Roles.Any(x => x.Value == "AdminAccess"))
            //{
            //    AddError("Você não tem permissão para adicionar usuários");
            //    return Response();
            //}

            var hashedPassword = _passwordService.HashPassword(request.Password);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                AddDate = DateTime.Now,
                Access = String.Join(";", request.Access),
                Password = hashedPassword,
                Type = request.Type,
            };

            var userAlreadyExistsResult = new UniqueUserEmailSpec(_userRepository).Validate(user);
            if (!userAlreadyExistsResult.IsValid) return Response(userAlreadyExistsResult);

            await _userRepository.AddUserAsync(user);

            await _activityLogHelper.LogAsync(ActivityLogType.AddUser, user.Id);

            return Response(new IdentityResult<int>(user.Id));
        }
    }
}
