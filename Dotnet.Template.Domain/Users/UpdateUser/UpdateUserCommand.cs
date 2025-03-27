using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Extensions;
using Dotnet.Template.Infra.Messaging;
using FluentValidation;
using System.Security.Claims;

namespace Dotnet.Template.Domain.Users
{
    public class UpdateUserCommand : Command<IdentityResult<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string[] Access { get; set; }

        public override bool IsValid()
        {
            var v = new InlineValidator<UpdateUserCommand>();
            v.RuleFor(x => x.Id).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource()).WithName("Id do usuário");
            ValidationResult = v.Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
