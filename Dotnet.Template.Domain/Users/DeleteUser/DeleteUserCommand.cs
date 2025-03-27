using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Extensions;
using Dotnet.Template.Infra.Messaging;
using FluentValidation;

namespace Dotnet.Template.Domain.Users
{
    public class DeleteUserCommand : Command<IdentityResult<int>>
    {
        public int Id { get; set; }

        public override bool IsValid()
        {
            var v = new InlineValidator<DeleteUserCommand>();
            v.RuleFor(x => x.Id).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource()).WithName("Id do usuário");
            ValidationResult = v.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
