using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Extensions;
using Dotnet.Template.Infra.Paging;
using FluentValidation;

namespace Dotnet.Templates.Domain.ActivityLogs
{
    public class GetActivityLogsCommand : CommandWithFilter<GetActivityLogsFilter, GetActivityLogsCommandResult>
	{
		public override bool IsValid()
		{
			var v = new InlineValidator<GetActivityLogsCommand>();
			v.RuleFor(x => x.Filter).NotEmpty().WithErrorCode(GlobalizationConstants.BASIC0001).WithMessage(GlobalizationConstants.BASIC0001.Resource());
			ValidationResult = v.Validate(this);
			return ValidationResult.IsValid;
		}
	}
}