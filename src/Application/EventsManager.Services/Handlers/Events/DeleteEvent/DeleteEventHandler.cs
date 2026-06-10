using EventManager.Application.Handlers;
using EventManager.Application.Repositories;
using EventManager.Domain.Events;
using EventsManager.Shared;

namespace EventManager.Application.Handlers.Events.DeleteEvent
{
    public class DeleteEventHandler : ICommandHandler<string, DeleteEventCommand>
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<string> HandleAsync(
            DeleteEventCommand command, 
            CancellationToken cancellationToken)
        {
            Guid id = command.Id;

            EventEntity? @event = await _eventsRepository.GetByIdAsync(id, cancellationToken);
            NullChecker.Check(@event);

            await _eventsRepository.DeleteAsync(id, cancellationToken);

            return $"Event with id = {id} had been deleted!";
        }

        public DeleteEventHandler(
            IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
