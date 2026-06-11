using EventManager.Domain.Failures;
using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Errors.Factories;
using EventManager.Domain.Failures.Exceptions.WebApi;
using EventsManager.Failures.Errors;

namespace EventManager.Domain.Failures.Exceptions.WebApi.Client.NotFound
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
