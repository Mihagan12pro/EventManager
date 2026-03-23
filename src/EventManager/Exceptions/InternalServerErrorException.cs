using EventManager.Shared;

namespace EventManager.Exceptions
{
    public class InternalServerErrorException : WebApiException
    {
        public InternalServerErrorException(string message) : base(message)
        {
            Error = Error.CreateError500(message);
        }
    }
}
