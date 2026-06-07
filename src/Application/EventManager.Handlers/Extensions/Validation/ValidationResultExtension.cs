using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventsManager.Failures.Errors;
using EventsManager.Failures.Errors.Collections;
using FluentValidation.Results;

namespace EventManager.Handlers.Extensions.Validation
{
    internal static class ValidationResultExtensions
    {
        /// <summary>
        /// Throws BadRequestException when appears validation fails
        /// </summary>
        /// <param name="validationResult"></param>
        /// <exception cref="BadRequestException"></exception>
        public static void ThrowIfNotIsValid(this ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                ErrorsCollection errors = new ErrorsCollection(validationResult.Errors.Select(vf => vf.ToError()));

                throw new BadRequestException(errors);
            }
        }
    }
}
