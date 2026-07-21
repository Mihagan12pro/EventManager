using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Application.Singleton.Cache.Options;
using Events.Domain;
using Events.Infrastracture.Entities;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace Events.Infrastracture.Repositories.Cache
{
    internal class RedisReadThrowRepository : ICacheRepository
    {
        private readonly IDatabase _redis;
        private readonly IReadEventsRepository _eventsRepository;
        private readonly CacheKeysOptions _cacheKeysOptions;

        public async Task<IEnumerable<Event>> GetMostPopularAsync(
            int count,
            CancellationToken cancellationToken)
        {
            var key = _cacheKeysOptions.TopEventsKey.FormatKey(count);

            var cached = await _redis.StringGetAsync(key);
            if (cached.HasValue)
                return JsonSerializer.Deserialize<IEnumerable<EventEntity>>(cached.ToString()).Select(e => EventEntity.ExtractEvent(e));

            var events = await _eventsRepository.GetMostPopularAsync(count, cancellationToken);

            if (events != null)
            {
                var serialized = JsonSerializer.Serialize(events.Select(e => EventEntity.ExtractEntity(e)));
                await _redis.StringSetAsync(key, serialized, _cacheKeysOptions.TopEventsKey.Expiry);

                return events;
            }

            return null;
        }

        public async Task<Event> GetEventAsync(
            Guid id, 
            CancellationToken cancellationToken)
        {
            var key = _cacheKeysOptions.GetEventKey.FormatKey(id);

            var cached = await _redis.StringGetAsync(key);
            if (cached.HasValue)
                return EventEntity.ExtractEvent(JsonSerializer.Deserialize<EventEntity>(cached.ToString()));

            var @event = await _eventsRepository.GetEventAsync(id, cancellationToken);
            var entity = EventEntity.ExtractEntity(@event);

            var serialized = JsonSerializer.Serialize(entity);
            await _redis.StringSetAsync(
                key, 
                
                serialized,
                
                _cacheKeysOptions.GetEventKey.Expiry
            );

            return @event;
        }

        public async Task RemoveAsync(
            string key,
            CancellationToken cancellationToken)
                => await _redis.KeyDeleteAsync(key);

        public async Task<bool> CheckKeyAsync(
            string key, 
            CancellationToken cancellationToken)
        {
            var value = await _redis.StringGetAsync(key);

            return value.HasValue;
        }

        public async Task AddEventAsync(
            string key,
            Event @event,
            CancellationToken cancellationToken)
        {
            string serialized = JsonSerializer.Serialize(
                EventEntity.ExtractEntity(@event)
            );

            await _redis.StringSetAsync(
                key,
                serialized,
                _cacheKeysOptions.GetEventKey.Expiry);
        }

        public async Task AddTopEventsAsync(
            string key,
            IEnumerable<Event> events,
            CancellationToken cancellationToken)
        {
            string serialized = JsonSerializer.Serialize(
                events.Select(e => EventEntity.ExtractEntity(e))
            );

            await _redis.StringSetAsync(
                key,
                serialized,
                _cacheKeysOptions.TopEventsKey.Expiry
            );
        }

        public RedisReadThrowRepository(
            IOptions<CacheKeysOptions> options,
            IConnectionMultiplexer connection,
            IReadEventsRepository eventsRepository)
        {
            _cacheKeysOptions = options.Value;

            _redis = connection.GetDatabase();

            _eventsRepository = eventsRepository;
        }
    }
}
