using EventsManager.Failures;
using EventsManager.Failures.Errors;
using EventsManager.Failures.Errors.Collections;
using EventsManager.Failures.Errors.ErrorsFactory;
using EventsManager.Failures.Errors.Factories.Server;
using EventsManager.Failures.Exceptions.WebApi;

namespace EventManager.Services.Exceptions.WebApi.Server.InternalServerError
{
    public class InternalServerErrorException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        internal override HttpErrorWorkbench HttpErrorWorkbench => ServerErrorsFactory.InternalServerErrorWorkbench;

        public InternalServerErrorException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public InternalServerErrorException(Error error) : base(error)
             => Error = HttpErrorWorkbench.Craft(error);

        public InternalServerErrorException(string message = "Internal server error!") : base(message)
             => Error = HttpErrorWorkbench.Craft(message);
    }
}
