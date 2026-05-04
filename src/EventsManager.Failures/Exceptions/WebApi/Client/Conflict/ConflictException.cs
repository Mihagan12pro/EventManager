using EventsManager.Failures;
using EventsManager.Failures.Errors;
using EventsManager.Failures.Errors.Collections;
using EventsManager.Failures.Errors.ErrorsFactory;
using EventsManager.Failures.Errors.ErrorsFactory.Client;
using EventsManager.Failures.Exceptions.WebApi;

namespace EventManager.Services.Exceptions.WebApi.Client.Conflict
{
    public class ConflictException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        internal override HttpErrorWorkbench HttpErrorWorkbench => ClientErrorsFactory.ConflictWorkbench;

        public ConflictException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public ConflictException(Error error) : base(error)
            => Error = HttpErrorWorkbench.Craft(error);

        public ConflictException(string message = "Conflict!") : base(message)
            => Error = HttpErrorWorkbench.Craft(message);
    }
}
