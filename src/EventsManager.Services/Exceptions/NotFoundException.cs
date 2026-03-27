using EventManager.Shared;

namespace EventManager.Exceptions
{
    public class NotFoundException : WebApiException
    {
        public NotFoundException(string message) : base(message)
        {
            Error = Error.CreateError404(message);
        }
    }
}
