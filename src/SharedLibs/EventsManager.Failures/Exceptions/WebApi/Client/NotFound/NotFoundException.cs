using EventsManager.Failures;
using EventsManager.Failures.Errors;
using EventsManager.Failures.Errors.Collections;
using EventsManager.Failures.Errors.ErrorsFactory;
using EventsManager.Failures.Errors.ErrorsFactory.Client;
using EventsManager.Failures.Exceptions.WebApi;

namespace EventManager.Services.Exceptions.WebApi.Client.NotFound
{
    public class NotFoundException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        internal override HttpErrorWorkbench HttpErrorWorkbench => ClientErrorsFactory.NotFoundWorkbench;

        public NotFoundException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public NotFoundException(Error error) : base(error)
            => Error = HttpErrorWorkbench.Craft(error);

        public NotFoundException(string message = "Not found!") : base(message)
            => Error = HttpErrorWorkbench.Craft(message);
    }
}
