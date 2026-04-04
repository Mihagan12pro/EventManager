using Shared;

namespace EventManager.Services.Exceptions
{
    public abstract class WebApiException : Exception
    {
        public Error Error { get; protected set; }

        public WebApiException(string message)
        {
            
        }
    }
}
