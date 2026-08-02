using Shared.Failures.Errors;
using Shared.Failures.Errors.Factories;

namespace Shared.Failures.Exceptions.WebApi.ServerErrors
{
    public class ServiceUnavailableException
        : WebApiException
    {
        public override HttpError Error { get; protected set; }

        public override HttpErrorWorkbench HttpErrorWorkbench => ServerErrorsFactory.ServiceUnavailableWorkbench;

        public ServiceUnavailableException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public ServiceUnavailableException(Error error) : base(error)
             => Error = HttpErrorWorkbench.Craft(error);

        public ServiceUnavailableException(string message = "Service Unavailable!") : base(message)
             => Error = HttpErrorWorkbench.Craft(message);
    }
}
