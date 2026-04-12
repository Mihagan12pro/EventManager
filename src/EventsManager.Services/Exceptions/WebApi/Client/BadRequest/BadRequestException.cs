using Shared;

namespace EventManager.Services.Exceptions.WebApi.Client.BadRequest
{
    public class BadRequestException : WebApiException
    {
        public BadRequestException(string message) : base(message)
        {
            Error = Error.CreateError400(message);
        }
    }
}
