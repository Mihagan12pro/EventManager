using EventManager.Application.Repositories;

namespace EventManager.Application.Handlers.Events.AddEvent
{
    internal class AddEventHandler : ICommandHandler<Guid, AddEventCommand>
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
