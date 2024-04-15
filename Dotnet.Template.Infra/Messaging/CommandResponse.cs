using FluentValidation.Results;

namespace Dotnet.Template.Infra.Messaging
{
    public class CommandResponse<TResult>
    {
        public CommandResponse(TResult result, ValidationResult validationResult)
        {
            Result = result;
            Errors = validationResult == null ? Array.Empty<ValidationFailure>() : validationResult.Errors.ToArray();
            ValidationResult = validationResult;
        }

        public bool IsValid => !Errors.Any();

        public ValidationResult ValidationResult { get; private set; }

        public TResult Result { get; private set; }

        public IEnumerable<ValidationFailure> Errors { get; private set; }
    }
}
