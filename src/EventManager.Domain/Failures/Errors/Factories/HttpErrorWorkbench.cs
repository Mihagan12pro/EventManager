using EventManager.Domain.Failures;
using EventManager.Domain.Failures.Errors;
using EventsManager.Failures.Errors;
using System.Net;

namespace EventManager.Domain.Failures.Errors.Factories
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

        internal HttpErrorWorkbench(HttpStatusCode statusCode)
        {
            _statusCode = statusCode;
        }
    }
}
