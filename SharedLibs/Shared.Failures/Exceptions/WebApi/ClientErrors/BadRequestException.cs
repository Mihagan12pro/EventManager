using Shared.Failures.Errors;
using Shared.Failures.Errors.Factories;

namespace Shared.Failures.Exceptions.WebApi.ClientErrors
{
    public class BadRequestException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        public override HttpErrorWorkbench HttpErrorWorkbench
            => ClientErrorsFactory.BadRequestWorkbench;

        public BadRequestException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public BadRequestException(Error error) : base(error)
            => Error = HttpErrorWorkbench.Craft(error);

        public BadRequestException(string message = "Bad request!") : base(message)
            => Error = HttpErrorWorkbench.Craft(message);
    }
}
