namespace Dotnet.Template.Domain
{
    public class IdentityResult<T>(T id) : IIdentityResult
    {
        public T Id { get; private set; } = id;
    }

    public interface IIdentityResult { }
}
