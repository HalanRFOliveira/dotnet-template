﻿using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Dotnet.Template.Infra.Messaging
{
    public abstract class Command<TResult> : IRequest<CommandResponse<TResult>>
    {
        protected ValidationResult ValidationResult { get; set; }

        public virtual bool IsValid()
        {
            return true;
        }

        public ValidationResult GetValidationResult()
        {
            return ValidationResult;
        }
    }
}
