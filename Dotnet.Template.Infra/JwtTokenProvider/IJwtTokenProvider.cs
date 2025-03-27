namespace Dotnet.Template.Infra.JwtTokenProvider
{
    public interface IJwtTokenProvider
    {
        string GenerateJwtToken(TokenData user, int expiresIn);
    }
}
