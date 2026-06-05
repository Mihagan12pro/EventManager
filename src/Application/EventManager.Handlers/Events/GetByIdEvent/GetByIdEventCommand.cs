namespace EventManager.Handlers.Events.GetByIdEvent
{
    public record GetByIdEventCommand(Guid Id) : ICommand;
}
