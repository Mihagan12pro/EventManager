using EventsManager.Failures.Errors;
using EventsManager.Failures.Exceptions.WebApi;

namespace EventManager.Services.Exceptions.WebApi.Client.NotFound
{
    public class NotFoundException : WebApiException
    {
        public NotFoundException(string message = "Not found!") : base(message)
        {
            Error = ClientErrors.Create404(message);
        }
    }
}
