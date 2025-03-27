using Dotnet.Template.Domain.Users;
using Dotnet.Template.Infra.Mediator;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Dotnet.Template.Domain.ActivityLogs
{
    public class ActivityLogHelper(
            IMediatorHandler mediator,
            IHttpContextAccessor httpContextAccessor,
            IUserRepository userRepository)
    {
		private readonly IMediatorHandler _mediator = mediator;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
		private readonly IUserRepository _userRepository = userRepository;

        public async Task LogAsync(ActivityLogType type, int objectRef, string details = null, int? userId = null)
		{
			userId ??= TryGetUserIdFromLoggedUser();
			await _mediator.SendCommandAsync(
				new AddActivityLogCommand
				{
					UserId = userId,
					TypeId = type,
					Details = details,
					ObjectRef = objectRef
				}
			);
		}

		private int? TryGetUserIdFromLoggedUser()
		{
			var user = _httpContextAccessor.HttpContext?.User;
			if (user == null) return null;

			var name = user.FindFirstValue(ClaimTypes.Name);
			var email = user.FindFirstValue(ClaimTypes.Email);

			return _userRepository.GetUserFromNameOrEmail(name, email)?.Id;
		}
	}
}
