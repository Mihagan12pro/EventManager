namespace EventManager.Application.Handlers.Bookings.Cancel
{
    public record CancelBookingCommand(Guid BookingId) : ICommand;
}
