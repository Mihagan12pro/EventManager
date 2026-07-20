using Events.Application.Dtos;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.CompleteUpdate
{
    public record CompleteEventUpdateCommand(
        Guid EventId,
        UpdateEventDto UpdateEvent
    ) : ICommand;
}
