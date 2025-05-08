using Dotnet.Template.Domain.Globalization;
using Dotnet.Template.Infra.Messaging;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Dotnet.Template.WebApi.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected ActionResult CustomResponse<TResult>(CommandResponse<TResult> response)
        {
            if (response == null)
            {
                return NoContent();
            }

            if (!response.IsValid)
            {
                return InvalidResponse(response);
            }

            if (IsNullable(response.Result) && response.Result == null)
            {
                return NoContent();
            }

            if (IsResultAnEmptyList(response))
            {
                return NoContent();
            }

            return Ok(response.Result);
        }

        protected ActionResult CustomCreatedResponse<TResult>(CommandResponse<TResult> response, Func<TResult, string> getUri)
        {
            if (!response.IsValid)
            {
                return InvalidResponse(response);
            }

            if (response == null) throw new ArgumentNullException(nameof(response));
            if (getUri == null) throw new ArgumentNullException(nameof(getUri));
            if (response.Result == null) throw new ArgumentNullException("response.Result");

            return Created(getUri(response.Result), response.Result);
        }

        private static bool IsResultAnEmptyList<TResult>(CommandResponse<TResult> response)
        {
            var list = response.Result as IEnumerable;
            return list != null && !list.Cast<object>().Any();
        }

        private ActionResult InvalidResponse<TResult>(CommandResponse<TResult> response)
        {
            if (response.Errors.Any(e => string.Equals(e.ErrorCode, GlobalizationConstants.NotFound)))
            {
                return NotFound();
            }

            var errors = FormatErrorMessages(response.Errors);

            return UnprocessableEntity(new ValidationProblemDetails
            (
                new Dictionary<string, string[]>
                {
                    {
                      "Messages"
                      , errors
                    }
                })
            );
        }

        private static string[] FormatErrorMessages(IEnumerable<ValidationFailure> errors)
        {
            return errors
              .Select(e =>
                string.IsNullOrWhiteSpace(e.ErrorCode) ?
                e.ErrorMessage :
                $"{e.ErrorMessage}")
              .ToArray();
        }

        private static bool IsNullable<T>(T obj)
        {
            if (obj == null) return true;
            Type type = typeof(T);
            if (!type.IsValueType) return true;
            if (Nullable.GetUnderlyingType(type) != null) return true;
            return false;
        }
    }
}
