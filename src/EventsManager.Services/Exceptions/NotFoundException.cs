using Shared;

namespace EventManager.Services.Exceptions
{
    public class NotFoundException : WebApiException
    {
        public NotFoundException(string message) : base(message)
        {
            Error = Error.CreateError404(message);
        }
    }
}
