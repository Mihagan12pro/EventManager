using EventsManager.Failures.Errors;
using FluentValidation.Results;

namespace EventManager.Services.Extensions.Validation
{
    internal static class ValidationFailureExtensions
    {
        public static Error ToError(this ValidationFailure failure)
            => new Error(failure.ErrorMessage);
    }
}
