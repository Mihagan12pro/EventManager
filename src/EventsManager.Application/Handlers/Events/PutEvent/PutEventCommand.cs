using EventManager.Application;
using EventManager.DTOs.Events;

namespace EventManager.Application.Handlers.Events.PutEvent
{
    public record PutEventCommand(
        Guid Id,
        PutEventDto PutEvent
    ) : ICommand;
}
