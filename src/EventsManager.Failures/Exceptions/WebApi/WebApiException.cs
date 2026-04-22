using EventsManager.Failures;

namespace EventsManager.Failures.Exceptions.WebApi
{
    public abstract class WebApiException : Exception
    {
        public HttpError Error { get; protected set; }

        public WebApiException(string message)
        {
            
        }
    }
}
