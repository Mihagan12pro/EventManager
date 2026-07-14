using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.Cancel
{
    public record CancelEventCommand(Guid Id) : ICommand;
}
