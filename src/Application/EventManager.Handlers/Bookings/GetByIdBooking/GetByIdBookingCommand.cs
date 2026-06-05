namespace EventManager.Handlers.Bookings.GetByIdBooking
{
    public record GetByIdBookingCommand(Guid BookingId) : ICommand;
}
