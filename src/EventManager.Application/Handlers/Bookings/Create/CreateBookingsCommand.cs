using EventManager.Application;

namespace EventManager.Application.Handlers.Bookings.Create
{
    public record CreateBookingCommand(Guid EventId) : ICommand;
}
