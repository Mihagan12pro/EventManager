using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Events.Domain;
using Events.Infrastracture.Entities;
using StackExchange.Redis;
using System.Text.Json;

namespace Events.Infrastracture.Repositories.Cache
{
    internal class RedisRepository : ICacheRepository
    {
        private readonly IDatabase _redis;

        private readonly IReadEventsRepository _eventsRepository;

        private readonly EventsDbContext _dbContext;

        public async Task<IEnumerable<Event>> GetMostPopularAsync(
            int count,
            CancellationToken cancellationToken)
        {
            var key = $"events:top:{count}";

            var cached = await _redis.StringGetAsync(key);
            if (cached.HasValue)
                return JsonSerializer.Deserialize<IEnumerable<EventEntity>>(cached.ToString()).Select(e => EventEntity.ExtractEvent(e));

            var events = await _eventsRepository.GetMostPopularAsync(10, cancellationToken);

            if (events != null)
            {
                var serialized = JsonSerializer.Serialize(events.Select(e => EventEntity.ExtractEntity(e)));
                await _redis.StringSetAsync(key, serialized, TimeSpan.FromMinutes(5));

                return events;
            }

            return null;
        }

        public async Task<Event> GetEventAsync(
            Guid id, 
            CancellationToken cancellationToken)
        {
            var key = $"events:event:{id}";

            var cached = await _redis.StringGetAsync(key);
            if (cached.HasValue)
                return EventEntity.ExtractEvent(JsonSerializer.Deserialize<EventEntity>(cached.ToString()));

            var @event = await _eventsRepository.GetEventAsync(id, cancellationToken);
            var entity = EventEntity.ExtractEntity(@event);

            var serialized = JsonSerializer.Serialize(entity);
            await _redis.StringSetAsync(key, serialized, TimeSpan.FromMinutes(1));

            return @event;
        }

        public async Task RemoveAsync(
            string key,
            CancellationToken cancellationToken)
                => await _redis.KeyDeleteAsync(key);

        public RedisRepository(
            EventsDbContext dbContext,
            IConnectionMultiplexer connection,
            IReadEventsRepository eventsRepository)
        {
            _dbContext = dbContext;

            _redis = connection.GetDatabase();

            _eventsRepository = eventsRepository;
        }
    }
}
