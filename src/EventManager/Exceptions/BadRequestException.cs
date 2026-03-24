using EventManager.Shared;

namespace EventManager.Exceptions
{
    public class BadRequestException : WebApiException
    {
        public BadRequestException(string message) : base(message)
        {
            Error = Error.CreateError400(message);
        }
    }
}
