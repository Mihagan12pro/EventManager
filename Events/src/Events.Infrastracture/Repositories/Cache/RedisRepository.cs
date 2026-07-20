using Events.Application.Repositories.Cache;
using Events.Domain;

namespace Events.Infrastracture.Repositories.Cache
{
    internal class RedisRepository : ICacheRepository
    {
        public async Task<IEnumerable<Event>> GetMostPopularAsync(
            int count,
            CancellationToken cancellationToken)
        {
            return null;
        }
    }
}
