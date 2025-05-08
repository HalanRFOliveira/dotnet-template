using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Extensions;
using Dotnet.Template.Infra.Messaging;
using FluentValidation;

namespace Dotnet.Template.Domain.Auth
{
    public class ResetPasswordCommand : Command<SendByEmailCommandResult>
    {
        public string Password { get; set; }
        public string Token { get; set; }

        public override bool IsValid()
        {
            var v = new InlineValidator<ResetPasswordCommand>();
            v.RuleFor(x => x.Password).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource());
            v.RuleFor(x => x.Token).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource());
            ValidationResult = v.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
