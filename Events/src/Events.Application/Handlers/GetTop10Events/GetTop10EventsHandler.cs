using Events.Application.Dtos;
using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Application.Singleton.Cache.Options;
using Microsoft.Extensions.Options;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.GetTop10Events
{
    internal class GetTop10EventsHandler : ICommandHandler<IEnumerable<GetEventDto>, GetTop10EventsCommand>
    {
        private readonly IReadEventsRepository _eventsRepository;

        private readonly ICacheRepository _cacheRepository;

        private readonly CacheKeysOptions _cacheKeysOptions;

        public async Task<IEnumerable<GetEventDto>> HandleAsync(
            GetTop10EventsCommand command,
            CancellationToken cancellationToken)
        {
            var events = await _cacheRepository.GetMostPopularAsync(10, cancellationToken);

            if (events == null)
            {
                events = await _eventsRepository.GetMostPopularAsync(10, cancellationToken);

                await _cacheRepository.AddTopEventsAsync(
                    _cacheKeysOptions.TopEventsKey.FormatKey(10),
                    events,     
                    cancellationToken
                );
            }

            return events.Select(e => new GetEventDto(
                e.Id, 
                e.Title, 
                e.StartAt,
                e.EndAt,
                e.Description,
                e.TotalSeats,
                e.AvailableSeats)
            );
        }

        public GetTop10EventsHandler(
            ICacheRepository cacheRepository,
            IReadEventsRepository eventsRepository,
            IOptions<CacheKeysOptions> options)
        {
            _cacheKeysOptions = options.Value;

            _cacheRepository = cacheRepository;

            _eventsRepository = eventsRepository;
        }
    }
}
