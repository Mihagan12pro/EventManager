using Events.Domain;

namespace Events.Application.Repositories.Cache
{
    public interface ICacheRepository
    {
        /// <summary>
        /// Implements Read-Throw
        /// </summary>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<Event>> GetMostPopularAsync(
            int count,
            CancellationToken cancellationToken
        );
    }
}
