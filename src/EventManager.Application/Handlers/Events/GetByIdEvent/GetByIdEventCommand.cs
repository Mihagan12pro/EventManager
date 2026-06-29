using EventManager.Application;

namespace EventManager.Application.Handlers.Events.GetByIdEvent
{
    public record GetByIdEventCommand(Guid Id) : ICommand;
}
