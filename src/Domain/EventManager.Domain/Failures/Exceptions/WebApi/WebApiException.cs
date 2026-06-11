using EventManager.Domain.Failures;
using EventManager.Domain.Failures.Errors;
using EventManager.Domain.Failures.Errors.Factories;
using EventsManager.Failures.Errors;

namespace EventManager.Domain.Failures.Exceptions.WebApi
{
    public abstract class WebApiException : Exception
    {
        public abstract HttpError Error { get; protected set; }

        public WebApiException(ErrorsCollection errors)
        {
            
        }

        public WebApiException(Error error)
        {

        }

        public WebApiException(string message = "")
        {
            
        }

        internal abstract HttpErrorWorkbench HttpErrorWorkbench { get; }
    }
}
