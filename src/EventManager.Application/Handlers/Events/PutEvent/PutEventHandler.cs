using EventManager.Application.Repositories;
using EventManager.Domain.Entities.Events;
using EventManager.DTOs.Events;
using EventManager.Shared;

namespace EventManager.Application.Handlers.Events.PutEvent
{
    internal class PutEventHandler : ICommandHandler<string, PutEventCommand>
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<string> HandleAsync(
            PutEventCommand command, 
            CancellationToken cancellationToken)
        {
            Guid id = command.Id;
            PutEventDto putEvent = command.PutEvent;


            EventEntity? eventById = await _eventsRepository.GetByIdAsync(id, cancellationToken);

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
