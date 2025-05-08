using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Extensions;
using Dotnet.Template.Infra.Messaging;
using FluentValidation;

namespace Dotnet.Template.Domain.Auth
{
    public class SendEmailResetPasswordCommand : Command<SendByEmailCommandResult>
    {
        public string Email { get; set; }

        public override bool IsValid()
        {
            var v = new InlineValidator<SendEmailResetPasswordCommand>();
            v.RuleFor(x => x.Email).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource());
            ValidationResult = v.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
