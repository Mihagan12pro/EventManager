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

        /// <summary>
        /// Implements Read-Throw
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Event> GetEventAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// Removes the specified key
        /// </summary>
        /// <returns></returns>
        Task RemoveAsync(
            string key,
            CancellationToken cancellationToken);
    }
}
