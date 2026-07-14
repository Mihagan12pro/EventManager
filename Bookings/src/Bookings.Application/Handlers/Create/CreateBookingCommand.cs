using Shared.Objects.Interfaces;

namespace Bookings.Application.Handlers.Create
{
    public record CreateBookingCommand(Guid Id) : ICommand;
}
