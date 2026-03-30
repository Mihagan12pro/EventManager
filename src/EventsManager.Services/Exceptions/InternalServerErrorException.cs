using Shared;

namespace EventManager.Services.Exceptions
{
    public class InternalServerErrorException : WebApiException
    {
        public InternalServerErrorException(string message) : base(message)
        {
            Error = Error.CreateError500(message);
        }
    }
}
