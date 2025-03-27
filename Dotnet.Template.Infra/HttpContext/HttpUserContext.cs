using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Dotnet.Template.Infra.HttpContext
{
    public class HttpUserContext(
        IHttpContextAccessor httpContextAccessor) : IHttpUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string TryGetEmailFromLoggedUser()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return null;

            var email = user.FindFirstValue(ClaimTypes.Email);

            return email;
        }
    }
}
