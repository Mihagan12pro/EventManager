using EventsManager.Failures;
using EventsManager.Failures.Errors;
using EventsManager.Failures.Errors.Collections;
using EventsManager.Failures.Errors.ErrorsFactory;

namespace EventsManager.Failures.Exceptions.WebApi
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
