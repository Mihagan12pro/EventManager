using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Services.Exceptions.WebApi.Client.Conflict
{
    public class ConflictException : WebApiException
    {
        public ConflictException(string message) : base(message)
        {
        }
    }
}
