using System.Net;

namespace Shared.Failures.Errors
{
    public class HttpErrorWorkbench
    {
        private readonly HttpStatusCode _statusCode;

        public HttpError Craft(string message = "There are some issues!")
            => new HttpError(_statusCode, message);

        public HttpError Craft(Error error)
            => new HttpError(_statusCode, error);

        public HttpError Craft(ErrorsCollection errors)
            => new HttpError(_statusCode, errors);

        public HttpErrorWorkbench(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }
    }
}
