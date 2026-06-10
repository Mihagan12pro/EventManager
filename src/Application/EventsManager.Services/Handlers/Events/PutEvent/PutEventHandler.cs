using EventManager.Application.Handlers;
using EventManager.Application.Repositories;
using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventsManager.Shared;

namespace EventManager.Application.Handlers.Events.PutEvent
{
    public class PutEventHandler : ICommandHandler<string, PutEventCommand>
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<string> HandleAsync(
            PutEventCommand command, 
            CancellationToken cancellationToken)
        {
            Guid id = command.Id;
            PutEventDto putEvent = command.PutEvent;


            EventEntity? eventById = await _eventsRepository.GetByIdAsync(id, cancellationToken);
            NullChecker.Check(eventById);

            await _eventsRepository.CompleteUpdateAsync(id, putEvent, cancellationToken);

            return $"Event with id = {id} had been updated!";
        }

        public PutEventHandler(
            IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
