using EventsManager.Failures.Errors;
using EventsManager.Failures.Exceptions.WebApi;

namespace EventManager.Services.Exceptions.WebApi.Client.BadRequest
{
    public class BadRequestException : WebApiException
    {
        public BadRequestException(string message = "Bad request!") : base(message)
        {
            Error = ClientErrors.Create400(message);
        }
    }
}
