namespace Dotnet.Template.Domain.Users
{
    public class GetUsersCommandResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public IEnumerable<string> Access { get; set; }
        public string Type { get; set; }
    }
}
