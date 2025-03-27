using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dotnet.Template.Infra.JwtTokenProvider
{
    public class JwtTokenProvider(IConfiguration configuration) : IJwtTokenProvider
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateJwtToken(TokenData user, int expiresIn)
        {
            var secretKey = _configuration.GetValue<string>("SecretKey");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var roles = new List<string>(); // user access roles if you going to use admin levels roles
            var claims = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    // Put here more claims if necessary
                ]);
            foreach (var role in roles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddDays(expiresIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
