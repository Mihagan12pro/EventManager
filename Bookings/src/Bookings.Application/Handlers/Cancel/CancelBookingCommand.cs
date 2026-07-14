using Shared.Objects.Interfaces;

namespace Bookings.Application.Handlers.Cancel
{
    public record CancelBookingCommand(Guid Id) : ICommand;
}
