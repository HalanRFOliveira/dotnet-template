using FluentValidation;
using FluentValidation.Results;

namespace Dotnet.Template.Infra.Validation
{
    public abstract class BaseSpec<T> : AbstractValidator<T>
	{
		public string NotSatisfiedCode { get; set; }

		public string NotSatisfiedReason { get; set; }

		/// <summary>
		/// Valida todos os erros de domínio.
		/// </summary>
		/// <param name="context">O contexto da validação</param>
		/// <returns>O resultado da validação</returns>
		public override ValidationResult Validate(ValidationContext<T> context)
		{
			var validationResult = new ValidationResult();

			if (context.InstanceToValidate != null)
			{
				validationResult = base.Validate(context);

                if (validationResult.Errors.Count == 0 && !this.IsSatisfiedBy(context.InstanceToValidate))
				{
					validationResult.Errors.Add(new ValidationFailure(string.Empty, NotSatisfiedReason) { ErrorCode = NotSatisfiedCode });
				}
			}

			return validationResult;
		}

		public abstract bool IsSatisfiedBy(T entity);
	}
}