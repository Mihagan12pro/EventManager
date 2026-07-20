using Events.Application.Dtos;
using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Domain;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.GetByIdEvent
{
    internal class GetByIdEventHandler : ICommandHandler<GetEventDto, GetByIdEventCommand>
    {
        private readonly ICacheRepository _cacheRepository;

        public async Task<GetEventDto> HandleAsync(
            GetByIdEventCommand command, 
            CancellationToken cancellationToken)
        {
            Event @event = await _cacheRepository.GetEventAsync(command.Id, cancellationToken);

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

        public GetByIdEventHandler(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }
    }
}
