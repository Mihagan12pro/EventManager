using Shared.Failures.Errors;
using Shared.Failures.Errors.Factories;

namespace Shared.Failures.Exceptions.WebApi.ClientErrors
{
    public class ForbiddenException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        public override HttpErrorWorkbench HttpErrorWorkbench => ClientErrorsFactory.ForbiddenWorkbench;

        public ForbiddenException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public ForbiddenException(Error error) : base(error)
            => Error = HttpErrorWorkbench.Craft(error);

        public ForbiddenException(string message = "Conflict!") : base(message)
            => Error = HttpErrorWorkbench.Craft(message);
    }
}
