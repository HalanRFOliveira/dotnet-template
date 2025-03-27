using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Paging;
using Dotnet.Template.Infra.Extensions;
using FluentValidation;

namespace Dotnet.Template.Domain.ActivityLogs
{
    public class GetActivityLogsCommand : CommandWithFilter<PagedFilter<ActivityLogType>, GetActivityLogsCommandResult>
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