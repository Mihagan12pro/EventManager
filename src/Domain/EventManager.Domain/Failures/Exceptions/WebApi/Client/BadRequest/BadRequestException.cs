using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Errors.Factories;
using EventsManager.Failures.Errors;

namespace EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest
{
    public class BadRequestException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        internal override HttpErrorWorkbench HttpErrorWorkbench => ClientErrorsFactory.BadRequestWorkbench;

        public BadRequestException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public BadRequestException(Error error) : base(error)
            => Error = HttpErrorWorkbench.Craft(error);

        public BadRequestException(string message = "Bad request!") : base(message)
            => Error = HttpErrorWorkbench.Craft(message);
    }
}
