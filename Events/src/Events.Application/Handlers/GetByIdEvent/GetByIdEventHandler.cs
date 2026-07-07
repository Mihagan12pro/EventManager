using Events.Application.Dtos;
using Events.Application.Repositories;
using Events.Domain;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.GetByIdEvent
{
    internal class GetByIdEventHandler : ICommandHandler<GetEventDto, GetByIdEventCommand>
    {
        private readonly IReadEventsRepository _readEventsRepository;

        public async Task<GetEventDto> HandleAsync(
            GetByIdEventCommand command, 
            CancellationToken cancellationToken)
        {
            Event @event = await _readEventsRepository.GetEventAsync(command.Id, cancellationToken);

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

        public GetByIdEventHandler(IReadEventsRepository readEventsRepository)
        {
            _readEventsRepository= readEventsRepository;
        }
    }
}
