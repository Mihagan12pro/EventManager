namespace EventManager.Handlers.Events.AddEvent
{
    public class AddEventHandler : ICommandHandler<Guid, AddEventCommand>
    {
        //private readonly I

        public Task<Guid> HandleAsync(
            in AddEventCommand command,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public AddEventHandler()
        {
            
        }
    }
}
