using Dotnet.Template.Infra.Messaging;

namespace Dotnet.Template.Domain.Users
{
    public class GetUserCommandHandler : CommandHandler<GetUserCommand, GetUsersCommandResult>
    {
        private readonly IUserRepository _userRepository;

        public GetUserCommandHandler(IUserRepository userRepository) : base()
        {
            _userRepository = userRepository;
        } 

        public override async Task<CommandResponse<GetUsersCommandResult>> Handle(GetUserCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return Response(request.GetValidationResult());

            var result = await _userRepository.GetUserAsync(request.Id);

            return Response(result);
        }
    }
}
