using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Extensions;
using Dotnet.Template.Infra.Validation;

namespace Dotnet.Template.Domain.Users
{
    public class UniqueUserEmailSpec(IUserRepository userRepository) : BaseSpec<User>
    {
        private readonly IUserRepository _userRepository = userRepository;

        public override bool IsSatisfiedBy(User user)
        {
            NotSatisfiedCode = GlobalizationConstants.UniqueUserEmailSpec;
            NotSatisfiedReason = GlobalizationConstants.UniqueUserEmailSpec.Resource();

            var userAlreadyExists = _userRepository.FindByEmail(user.Email);
            return userAlreadyExists == null || userAlreadyExists.Id == user.Id;
        }
    }
}
