using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Errors.Factories;
using EventsManager.Failures.Errors;

namespace EventManager.Domain.Failures.Exceptions.WebApi.Client.Forbidden
{
    public class ForbiddenException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        internal override HttpErrorWorkbench HttpErrorWorkbench => ClientErrorsFactory.ForbiddenWorkbench;

        public ForbiddenException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public ForbiddenException(Error error) : base(error)
            => Error = HttpErrorWorkbench.Craft(error);

        public ForbiddenException(string message = "Conflict!") : base(message)
            => Error = HttpErrorWorkbench.Craft(message);
    }
}
