using FluentValidation;
using Dotnet.Template.Infra.Messaging;
using Dotnet.Template.Infra.Extensions;
using Dotnet.Template.Domain.Globalization;

namespace Dotnet.Template.Domain.Users
{
    public class GetUserCommand : Command<GetUsersCommandResult>
    {
        public int Id { get; set; }

        public override bool IsValid()
        {
            var v = new InlineValidator<GetUserCommand>();
            v.RuleFor(x => x.Id).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource()).WithName("Id do usuário");
            ValidationResult = v.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
