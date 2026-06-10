using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.NotFound;
using FluentValidation.Results;

namespace EventManager.Handlers.Extensions.Validation
{
    internal static class ValidationResultExtensions
    {
        /// <summary>
        /// Throws BadRequestException when validation fails appears
        /// </summary>
        /// <param name="validationResult"></param>
        /// <exception cref="BadRequestException"></exception>
        public static void ThrowBadRequestIfNotIsValid(this ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                ErrorsCollection errors = new ErrorsCollection(validationResult.Errors.Select(vf => vf.ToError()));

                throw new BadRequestException(errors);
            }
        }

        public static void ThrowNotFoundIfNotIsValid(this ValidationResult validationResult, string message = "This resource does not exists!")
        {
            if (!validationResult.IsValid)
                throw new NotFoundException(message);
        }
    }
}
