using System;
using System.Collections.Generic;
using System.Text;

namespace EventManager.Services.Exceptions.WebApi.Client.Conflict
{
    public class NoAvailableSeatsException : ConflictException
    {
        public NoAvailableSeatsException(string message = "No available seats for this event") : base(message)
        {
        }
    }
}
