namespace Dotnet.Template.Infra.PasswordService
{
    public interface IPasswordService
    {
        public string HashPassword(string password);
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword);
    }
}
