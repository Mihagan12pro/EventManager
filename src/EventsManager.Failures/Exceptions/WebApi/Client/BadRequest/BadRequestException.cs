using EventsManager.Failures;
using EventsManager.Failures.Errors;
using EventsManager.Failures.Errors.Collections;
using EventsManager.Failures.Errors.ErrorsFactory;
using EventsManager.Failures.Errors.ErrorsFactory.Client;
using EventsManager.Failures.Exceptions.WebApi;

namespace EventManager.Services.Exceptions.WebApi.Client.BadRequest
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
