using Dotnet.Template.Infra.CrossCutting.Domain;

namespace Dotnet.Template.Domain.Users
{
    public class User : EntityBase<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Access { get; set; }
        public string Type { get; set; }

    }
}
