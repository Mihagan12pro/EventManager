using EventManager.Application;

namespace EventManager.Application.Handlers.Events.DeleteEvent
{
    public record DeleteEventCommand(Guid Id) : ICommand;
}
