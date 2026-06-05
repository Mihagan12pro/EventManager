using EventManager.Domain.Events;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;

namespace EventManager.Handlers.Events.DeleteEvent
{
    public class DeleteEventHandler : ICommandHandler<string, DeleteEventCommand>
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<string> HandleAsync(
            DeleteEventCommand command, 
            CancellationToken cancellationToken)
        {
            Guid id = command.Id;

            EventModel? @event = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (@event == null)
                throw new NotFoundException($"Event with id = {id} does not exists!");

            await _eventsRepository.DeleteAsync(id, cancellationToken);

            return $"Event with id = {id} had been deleted!";
        }

        public DeleteEventHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
