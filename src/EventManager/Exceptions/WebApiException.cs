using EventManager.Shared;

namespace EventManager.Exceptions
{
    public abstract class WebApiException : Exception
    {
        public Error Error { get; protected set; }

        public WebApiException(string message)
        {
            
        }
    }
}
