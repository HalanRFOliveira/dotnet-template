using Dotnet.Template.Infra.Messaging;
using Dotnet.Template.Infra.Paging;

namespace Dotnet.Template.Domain.Users
{
    public class GetUsersCommandHandler(IUserRepository userRepository) : CommandHandler<GetUsersCommand, PagedResult<GetUsersCommandResult>>()
    {
        private readonly IUserRepository _userRepository = userRepository;

        public override async Task<CommandResponse<PagedResult<GetUsersCommandResult>>> Handle(GetUsersCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return Response(request.GetValidationResult());

            var result = _userRepository.GetUsers(request.Filter);

            await Task.CompletedTask;
            return Response(result);
        }
    }
}
