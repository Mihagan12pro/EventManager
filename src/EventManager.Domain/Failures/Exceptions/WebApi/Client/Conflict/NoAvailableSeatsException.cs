namespace EventManager.Domain.Failures.Exceptions.WebApi.Client.Conflict
{
    public class NoAvailableSeatsException : ConflictException
    {
        public NoAvailableSeatsException(string message = "No available seats for this event") : base(message)
        {
        }
    }
}
