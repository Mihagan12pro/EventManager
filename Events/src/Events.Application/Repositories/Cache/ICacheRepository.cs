using Events.Domain;

namespace Events.Application.Repositories.Cache
{
    public interface ICacheRepository
    {
        Task<IEnumerable<Event>> GetMostPopularAsync(
            int count,
            CancellationToken cancellationToken
        );

        Task<Event> GetEventAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// Checks for the presence of the key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> CheckKeyAsync(
            string key,
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
