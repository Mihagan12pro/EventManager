namespace EventManager.Handlers.Events.DeleteEvent
{
    public record DeleteEventCommand(Guid Id) : ICommand;
}
