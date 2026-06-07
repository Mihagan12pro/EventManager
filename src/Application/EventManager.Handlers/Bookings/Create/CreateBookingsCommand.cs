namespace EventManager.Handlers.Bookings.Create
{
    public record CreateBookingCommand(Guid EventId) : ICommand;
}
