using EventsManager.Failures.Errors;
using EventsManager.Failures.Exceptions.WebApi;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Services.Exceptions.WebApi.Client.Conflict
{
    public class ConflictException : WebApiException
    {
        public ConflictException(string message = "Conflict!") : base(message)
        {
            Error = ClientErrors.Create409(message);
        }
    }
}
