using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Domain;
using StackExchange.Redis;
using System.Text.Json;

namespace Events.Infrastracture.Repositories.Cache
{
    internal class RedisRepository : ICacheRepository
    {
        private readonly IDatabase _redis;

        private readonly IReadEventsRepository _eventsRepository;

        public async Task<IEnumerable<Event>> GetMostPopularAsync(
            int count,
            CancellationToken cancellationToken)
        {
            var key = $"top:{count}";

            var cached = await _redis.StringGetAsync(key);
            if (cached.HasValue)
                return JsonSerializer.Deserialize<IEnumerable<Event>>(cached.ToString());

            var events = await _eventsRepository.GetMostPopularAsync(10, cancellationToken);

            if (events != null)
            {
                var serialized = JsonSerializer.Serialize(events);
                await _redis.StringSetAsync(key, serialized, TimeSpan.FromMinutes(5));

                return events;
            }

            return null;
        }

        public RedisRepository(
            IConnectionMultiplexer connection,
            IReadEventsRepository eventsRepository)
        {
            _redis = connection.GetDatabase();

            _eventsRepository = eventsRepository;
        }
    }
}
