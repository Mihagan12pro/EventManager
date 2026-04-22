using System.Net;

namespace EventsManager.Failures
{
    public class HttpError
    {
        public HttpStatusCode StatusCode { get; }

        public string Message { get; }

        internal HttpError(
            HttpStatusCode statusCode, 
            string message)
        {
            StatusCode = statusCode;

            Message = message;
        }
    }
}
