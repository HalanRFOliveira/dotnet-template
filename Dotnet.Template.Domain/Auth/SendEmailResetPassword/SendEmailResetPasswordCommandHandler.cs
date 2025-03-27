using Dotnet.Template.Domain.ActivityLogs;
using Dotnet.Template.Domain.Users;
using Dotnet.Template.Infra.Email;
using Dotnet.Template.Infra.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dotnet.Template.Domain.Auth
{
    public class SendEmailResetPasswordCommandHandler(
        IUserRepository userRepository,
        IEmailService emailService,
        IConfiguration configuration,
        ActivityLogHelper activityLogHelper
            ) : CommandHandler<SendEmailResetPasswordCommand, SendByEmailCommandResult>()
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly IConfiguration _configuration = configuration;
        private readonly ActivityLogHelper _activityLogHelper = activityLogHelper;

        public override async Task<CommandResponse<SendByEmailCommandResult>> Handle(SendEmailResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid()) return Response(request.GetValidationResult());

            var user = _userRepository.FindByEmail(request.Email);

            if (user == null)
            {
                AddError("Email não cadastrado");
                return Response();
            };

            var token = GenerateToken(user);
            var baseUrl = _configuration.GetValue<string>("BaseUrl");

            try
            {
                var to = new List<string> { user.Email };
                var subject = "Recuperação de Senha";
                var message = $@"Clique no link a seguir para efetuar a recuperação de senha. Se não solicitou a recuperação, desconsidere este email.
                                {baseUrl}/resetpassword?token={token}";
                await _emailService.SendAsHtmlAsync(to, subject, message);
            }
            catch (Exception)
            {
                AddError("Houve um problema no envio do email de recuperação de senha");
            }

            await _activityLogHelper.LogAsync(ActivityLogType.ResetPasswordEmail, user.Id, user.Email);

            return Response(new SendByEmailCommandResult { Body = "Sucesso" });
        }

        private string GenerateToken(User user)
        {
            var secretKey = _configuration.GetValue<string>("ResetPasswordSecretKey");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var roles = user.Access.Split(";");
            var claims = new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            ]);
            foreach (var role in roles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
