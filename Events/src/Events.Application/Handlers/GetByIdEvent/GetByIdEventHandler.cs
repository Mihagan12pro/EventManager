using Events.Application.Dtos;
using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Application.Singleton.Cache.Options;
using Events.Domain;
using Microsoft.Extensions.Options;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.GetByIdEvent
{
    internal class GetByIdEventHandler : ICommandHandler<GetEventDto, GetByIdEventCommand>
    {
        private readonly IReadEventsRepository _eventsRepository;

        private readonly ICacheRepository _cacheRepository;

        private readonly CacheKeysOptions _cacheKeysOptions;

        public async Task<GetEventDto> HandleAsync(
            GetByIdEventCommand command, 
            CancellationToken cancellationToken)
        {
            Event @event = await _cacheRepository.GetEventAsync(command.Id, cancellationToken);

            if (@event == null)
            {
                @event = await _eventsRepository.GetEventAsync(command.Id, cancellationToken);

                await _cacheRepository.AddEventAsync(
                    _cacheKeysOptions.GetEventKey.FormatKey(@event.Id),
                    @event, 
                    cancellationToken
                );
            }

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

        public GetByIdEventHandler(
            ICacheRepository cacheRepository,
            IReadEventsRepository eventsRepository,
            IOptions<CacheKeysOptions> options)
        {
            _cacheKeysOptions = options.Value;

            _eventsRepository = eventsRepository;
            
            _cacheRepository = cacheRepository;
        }
    }
}
