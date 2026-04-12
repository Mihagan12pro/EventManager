using EventsManager.Failures.Errors;
using EventsManager.Failures.Exceptions.WebApi;

namespace EventManager.Services.Exceptions.WebApi.Server.InternalServerError
{
    public class InternalServerErrorException : WebApiException
    {
        public InternalServerErrorException(string message = "Internal server error!") : base(message)
        {
            Error = ServerErrors.Create500(message);
        }
    }
}
