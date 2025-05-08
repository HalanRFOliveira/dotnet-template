using Dotnet.Template.Domain.ActivityLogs;
using Dotnet.Template.Domain.Users;
using Dotnet.Template.Infra.Email;
using Dotnet.Template.Infra.Messaging;
using Dotnet.Template.Infra.PasswordService;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dotnet.Template.Domain.Auth
{
    public class ResetPasswordCommandHandler(
        IUserRepository userRepository,
        IConfiguration configuration,
        IPasswordService passwordService,
        ActivityLogHelper activityLogHelper,
        IEmailService emailService
            ) : CommandHandler<ResetPasswordCommand, SendByEmailCommandResult>()
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IConfiguration _configuration = configuration;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IEmailService _emailService = emailService;
        private readonly ActivityLogHelper _activityLogHelper = activityLogHelper;

        public override async Task<CommandResponse<SendByEmailCommandResult>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return Response(request.GetValidationResult());

            var token = request.Token;
            var secretKey = _configuration.GetValue<string>("ResetPasswordSecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var claims = tokenHandler.ValidateToken(token, validationParameters, out _);
                var userEmail = claims.FindFirstValue(ClaimTypes.Email);

                if (string.IsNullOrEmpty(userEmail))
                {
                    AddError("Token inválido.");
                    return Response();
                }

                var user = _userRepository.FindByEmail(userEmail);
                if (user == null)
                {
                    AddError("Email não cadastrado");
                    return Response();
                }

                var hashedPassword = _passwordService.HashPassword(request.Password);

                user.Password = hashedPassword;

                await _userRepository.UpdateUserAsync(user);
                await _activityLogHelper.LogAsync(ActivityLogType.UpdateUser, user.Id, "Redefinição de senha");

            }
            catch (SecurityTokenException)
            {
                AddError("Token inválido.");
                return Response();
            }


            return Response(new SendByEmailCommandResult { Body = "Sucesso" });
        }
    }
}
