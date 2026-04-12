using Shared;

namespace EventManager.Services.Exceptions.WebApi.Server.InternalServerError
{
    public class InternalServerErrorException : WebApiException
    {
        public InternalServerErrorException(string message) : base(message)
        {
            Error = Error.CreateError500(message);
        }
    }
}
