using Shared.Failures.Exceptions.WebApi.ClientErrors;

namespace Events.Domain.Exceptions
{
    public class NoAvailableSeatsException : ConflictException
    {
        public NoAvailableSeatsException(string message = "Conflict!") : base(message)
        {
        }
    }
}
