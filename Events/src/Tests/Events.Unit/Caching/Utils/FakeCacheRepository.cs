using Events.Application.Repositories.Cache;
using Events.Domain;

namespace Events.Unit.Caching.Utils
{
    internal class FakeCacheRepository : ICacheRepository
    {
        public readonly Dictionary<string, Event> EventsCache = new ();

        public async Task CacheEventAsync(
            string key, 
            Event @event,
            CancellationToken cancellationToken)
                => EventsCache.Add(key, @event);

        public async Task CacheTopEventsAsync(
            string key,
            IEnumerable<Event> events,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckKeyAsync(
            string key, 
            CancellationToken cancellationToken)
                => EventsCache.TryGetValue(key, out Event @event);

        public async Task<Event> GetEventAsync(
            Guid id, 
            CancellationToken cancellationToken)
        {
            EventsCache.TryGetValue($"events:event:{id}", out Event @event);

            return @event;
        }

        public async Task<IEnumerable<Event>> GetMostPopularAsync(
            int count, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveAsync(
            string key,
            CancellationToken cancellationToken)
                => EventsCache.Remove(key);
    }
}
