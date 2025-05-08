using Dotnet.Template.Domain.Users;
using Dotnet.Template.Infra.JwtTokenProvider;
using Dotnet.Template.Infra.Messaging;
using Dotnet.Template.Infra.PasswordService;
using Microsoft.Extensions.Configuration;

namespace Dotnet.Template.Domain.Auth
{
    public class GetAuthenticationCommandHandler(
        IUserRepository userRepository,
        IConfiguration configuration,
        IPasswordService passwordService,
        IJwtTokenProvider jwtTokenProvider
            ) : CommandHandler<GetAuthenticationCommand, GetAuthenticationCommandResult>()
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _configuration = configuration;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IJwtTokenProvider _jwtTokenProvider = jwtTokenProvider;

        public override async Task<CommandResponse<GetAuthenticationCommandResult>> Handle(GetAuthenticationCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return Response(request.GetValidationResult());

            var user = _userRepository.FindByEmail(request.Email);
            if (user == null)
            {
                AddError("Email não cadastrado");
                return Response();
            };


            var verifyHashedPasswrod = _passwordService.VerifyHashedPassword(user.Password, request.Password);

            var userVerified = user.Email == request.Email && verifyHashedPasswrod;
            if (!userVerified)
            {
                AddError("Email ou senha inválidos");
                return Response();
            }

            var userData = new TokenData
            {
                Name = user.Name,
                Email = user.Email,
                Access = user.Access,
            };

            var jwtToken = _jwtTokenProvider.GenerateJwtToken(userData, 1);

            await Task.CompletedTask;

            return Response(new GetAuthenticationCommandResult { Token = jwtToken });

        }

    }
}
