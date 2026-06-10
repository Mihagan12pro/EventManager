using EventManager.Domain.Failures;
using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Errors.Factories;
using EventManager.Domain.Failures.Exceptions.WebApi;
using EventsManager.Failures.Errors;

namespace EventManager.Domain.Failures.Exceptions.WebApi.Client.Conflict
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
