using Shared;

namespace EventManager.Services.Exceptions.WebApi.Client.NotFound
{
    public class NotFoundException : WebApiException
    {
        public NotFoundException(string message) : base(message)
        {
            Error = Error.CreateError404(message);
        }
    }
}
