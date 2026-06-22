using EventManager.Application.Repositories;
using EventManager.Domain.Entities.Events;
using EventManager.DTOs.Events;
using EventManager.Shared;

namespace EventManager.Application.Handlers.Events.GetByIdEvent
{
    internal class GetByIdEventHandler : ICommandHandler<GetEventDto, GetByIdEventCommand>
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<GetEventDto> HandleAsync(
            GetByIdEventCommand command, 
            CancellationToken cancellationToken)
        {
            Guid id = command.Id;

            EventEntity? @event = await _eventsRepository.GetByIdAsync(id, cancellationToken);

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
