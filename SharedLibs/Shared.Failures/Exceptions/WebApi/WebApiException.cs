using Shared.Failures.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Failures.Exceptions.WebApi
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

        public abstract HttpErrorWorkbench HttpErrorWorkbench { get; }
    }
}
