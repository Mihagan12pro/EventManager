using EventManager.Domain.Failures;
using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Errors.Factories;
using EventManager.Domain.Failures.Exceptions.WebApi;
using EventsManager.Failures.Errors;

namespace EventManager.Domain.Failures.Exceptions.WebApi.Server.InternalServerError
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
