using EventManager.Repositories.Events;

namespace EventManager.Handlers.Events.AddEvent
{
    public class AddEventHandler : ICommandHandler<Guid, AddEventCommand>
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<Guid> HandleAsync(
            AddEventCommand command,
            CancellationToken cancellationToken)
        {
            Guid id = await _eventsRepository.AddNewAsync(command.NewEvent, cancellationToken);

            return id;
        }

        public AddEventHandler(
            IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
