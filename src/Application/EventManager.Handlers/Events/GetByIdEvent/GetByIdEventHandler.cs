using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;

namespace EventManager.Handlers.Events.GetByIdEvent
{
    public class GetByIdEventHandler : ICommandHandler<GetEventDto, GetByIdEventCommand>
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<GetEventDto> HandleAsync(
            GetByIdEventCommand command, 
            CancellationToken cancellationToken)
        {
            Guid id = command.Id;

            EventModel? @event = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (@event == null)
                throw new NotFoundException($"Event with id = {id} does not exists!");

            return new GetEventDto(
                @event.Id,
                @event.Title,
                @event.StartAt,
                @event.EndAt,
                @event.Description,
                @event.TotalSeats,
                @event.AvailableSeats
            );
        }

        public GetByIdEventHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
