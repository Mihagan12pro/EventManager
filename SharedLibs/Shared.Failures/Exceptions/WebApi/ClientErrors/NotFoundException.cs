using Shared.Failures.Errors;
using Shared.Failures.Errors.Factories;

namespace Shared.Failures.Exceptions.WebApi.ClientErrors
{
    public class NotFoundException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        public override HttpErrorWorkbench HttpErrorWorkbench => ClientErrorsFactory.NotFoundWorkbench;

        public NotFoundException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public NotFoundException(Error error) : base(error)
            => Error = HttpErrorWorkbench.Craft(error);

        public NotFoundException(string message = "Not found!") : base(message)
            => Error = HttpErrorWorkbench.Craft(message);
    }
}
