using Shared.Failures.Errors;
using Shared.Failures.Errors.Factories;

namespace Shared.Failures.Exceptions.WebApi.ServerErrors
{
    public class InternalServerErrorException : WebApiException
    {
        public override HttpError Error { get; protected set; }

        public override HttpErrorWorkbench HttpErrorWorkbench => ServerErrorsFactory.InternalServerErrorWorkbench;

        public InternalServerErrorException(ErrorsCollection errors) : base(errors)
            => Error = HttpErrorWorkbench.Craft(errors);

        public InternalServerErrorException(Error error) : base(error)
             => Error = HttpErrorWorkbench.Craft(error);

        public InternalServerErrorException(string message = "Internal server error!") : base(message)
             => Error = HttpErrorWorkbench.Craft(message);
    }
}
