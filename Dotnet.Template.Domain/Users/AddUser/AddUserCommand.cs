using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Extensions;
using Dotnet.Template.Infra.Messaging;
using FluentValidation;

namespace Dotnet.Template.Domain.Users
{
    public class AddUserCommand : Command<IdentityResult<int>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string[] Access { get; set; }
        public string Type { get; set; }

        public override bool IsValid()
        {
            var v = new InlineValidator<AddUserCommand>();
            v.RuleFor(x => x.Name).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource());
            v.RuleFor(x => x.Email).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource());
            v.RuleFor(x => x.Password).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource());
            v.RuleFor(x => x.Access).NotNull().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource());
            ValidationResult = v.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
