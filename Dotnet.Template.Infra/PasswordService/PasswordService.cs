using Microsoft.AspNetCore.Identity;

namespace Dotnet.Template.Infra.PasswordService;

public class PasswordService(IPasswordHasher<object> passwordHasher) : IPasswordService
{
    private readonly IPasswordHasher<object> _passwordHasher = passwordHasher;

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null, password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
    }
}