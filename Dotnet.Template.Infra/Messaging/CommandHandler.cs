using FluentValidation.Results;
using MediatR;

namespace Dotnet.Template.Infra.Messaging
{
    public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, CommandResponse<TResult>>
      where TCommand : Command<TResult>
    {

        private readonly ValidationResult validationResult;

        protected CommandHandler()
        {
            validationResult = new ValidationResult();
        }

        protected bool IsValid => validationResult.IsValid;

        public abstract Task<CommandResponse<TResult>> Handle(TCommand request, CancellationToken cancellationToken);

        protected void AddError(string message)
        {
            validationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        protected void AddError(string errorCode, string message)
        {
            validationResult.Errors.Add(new ValidationFailure(string.Empty, message) { ErrorCode = errorCode });
        }

        protected CommandResponse<TResult> Response()
        {
            return new CommandResponse<TResult>(default, validationResult);
        }

        protected CommandResponse<TResult> Response(TResult result)
        {
            return new CommandResponse<TResult>(result, validationResult);
        }

        protected CommandResponse<TResult> Response(ValidationResult validationResult)
        {
            return new CommandResponse<TResult>(default, validationResult);
        }

    }
}
