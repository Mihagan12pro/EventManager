using EventManager.DTOs.Events;

namespace EventManager.Handlers.Events.PutEvent
{
    public record PutEventCommand(
        Guid Id,
        PutEventDto PutEvent
    ) : ICommand;
}
