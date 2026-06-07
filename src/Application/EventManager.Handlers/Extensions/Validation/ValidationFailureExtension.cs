using EventsManager.Failures.Errors;
using FluentValidation.Results;

namespace EventManager.Handlers.Extensions.Validation
{
    internal static class ValidationFailureExtension
    {
        public static Error ToError(this ValidationFailure failure)
           => new Error(failure.ErrorMessage);
    }
}
