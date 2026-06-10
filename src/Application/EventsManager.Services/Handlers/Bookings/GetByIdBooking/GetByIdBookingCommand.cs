using EventManager.Application;

namespace EventManager.Application.Handlers.Bookings.GetByIdBooking
{
    public record GetByIdBookingCommand(Guid BookingId) : ICommand;
}
