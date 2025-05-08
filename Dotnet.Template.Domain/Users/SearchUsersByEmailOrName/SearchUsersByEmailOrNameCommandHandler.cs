using Dotnet.Template.Infra.Messaging;

namespace Dotnet.Template.Domain.Users
{
    public class SearchUsersByEmailOrNameCommandHandler(IUserRepository user) : CommandHandler<SearchUsersByEmailOrNameCommand, IEnumerable<GetUsersCommandResult>>
    {
        private readonly IUserRepository _userRepository = user;
        public async override Task<CommandResponse<IEnumerable<GetUsersCommandResult>>> Handle(SearchUsersByEmailOrNameCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return Response(request.GetValidationResult());

            var response = _userRepository.SearchUsersByEmailOrName(request.Term);

            return Response(response);
        }
    }
}
