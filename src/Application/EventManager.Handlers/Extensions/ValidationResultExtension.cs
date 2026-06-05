using EventsManager.Failures.Errors;
using FluentValidation.Results;

namespace EventManager.Handlers.Extensions
{
    internal static class ValidationResultExtensions
    {
        public static Error ToError(this ValidationFailure failure)
           => new Error(failure.ErrorMessage);
    }
}
