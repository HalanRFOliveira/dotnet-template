namespace Dotnet.Template.Infra.HttpContext
{
    public interface IHttpUserContext
    {
        string TryGetEmailFromLoggedUser();
    }
}
