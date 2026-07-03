using Shared.Failures.Errors;
using Shared.Failures.Errors.Factories;

namespace Shared.Failures.Exceptions.WebApi.ClientErrors
{
    public class ConflictException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        public override HttpErrorWorkbench HttpErrorWorkbench => ClientErrorsFactory.ConflictWorkbench;

        public ConflictException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public ConflictException(Error error) : base(error)
            => Error = HttpErrorWorkbench.Craft(error);

        public ConflictException(string message = "Conflict!") : base(message)
            => Error = HttpErrorWorkbench.Craft(message);
    }
}
