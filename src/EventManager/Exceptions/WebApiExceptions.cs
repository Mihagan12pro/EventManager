using EventManager.Shared;

namespace EventManager.Exceptions
{
    public abstract class WebApiExceptions : Exception
    {
        public Error Error { get; protected set; }

        public WebApiExceptions(string message)
        {
            
        }
    }
}
