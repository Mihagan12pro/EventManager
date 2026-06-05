namespace EventManager.Handlers.Bookings.Create
{
    public record CreateBookingsCommand(Guid EventId) : ICommand;
}
