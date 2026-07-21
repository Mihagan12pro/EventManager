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
    internal class RedisCashAsideRepository : ICacheRepository
    {
        private readonly IDatabase _redis;
        private readonly CacheKeysOptions _cacheKeysOptions;

        public async Task<Event> GetEventAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            var key = _cacheKeysOptions.GetEventKey.FormatKey(id);

            var cached = await _redis.StringGetAsync(key);
            if (cached.HasValue)
                return EventEntity.ExtractEvent(JsonSerializer.Deserialize<EventEntity>(cached.ToString()));

            return null;
        }

        public async Task<IEnumerable<Event>> GetMostPopularAsync(
            int count,
            CancellationToken cancellationToken)
        {
            var key = _cacheKeysOptions.TopEventsKey.FormatKey(count);

            var cached = await _redis.StringGetAsync(key);
            if (cached.HasValue)
                return JsonSerializer.Deserialize<IEnumerable<EventEntity>>(cached.ToString()).Select(e => EventEntity.ExtractEvent(e));

            return null;
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

        public RedisCashAsideRepository(
            IOptions<CacheKeysOptions> options,
            IConnectionMultiplexer connection
            )
        {
            _cacheKeysOptions = options.Value;

            _redis = connection.GetDatabase();
        }
    }
}
