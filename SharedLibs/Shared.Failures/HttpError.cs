using Shared.Failures.Errors;
using System.Net;

namespace Shared.Failures
{
    public class HttpError
    {
        public HttpStatusCode StatusCode { get; }

        public ErrorsCollection Errors { get; }

        internal HttpError(
            HttpStatusCode statusCode,
            ErrorsCollection errors)
        {
            StatusCode = statusCode;

            Errors = errors;
        }

        internal HttpError(
            HttpStatusCode statusCode,
            Error error)
        {
            StatusCode = statusCode;

            Errors = new ErrorsCollection();
            Errors.Add(error);
        }

        internal HttpError(
            HttpStatusCode statusCode,
            string message)
        {
            StatusCode = statusCode;

            Errors = new ErrorsCollection();
            Errors.Add(new Error(message));
        }
    }
}
