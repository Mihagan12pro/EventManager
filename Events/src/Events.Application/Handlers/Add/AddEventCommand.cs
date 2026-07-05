using Events.Application.Contracts;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.Add
{
    public record AddEventCommand(NewEventDto NewEvent)
        : ICommand;
}
