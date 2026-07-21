using Events.Application.Dtos;
using Events.Domain;
using Shared.Objects.Classes.Collections;
using Shared.Objects.Records;

namespace Events.Application.Repositories.Events
{
    public interface IReadEventsRepository
    {
        Task<IEnumerable<Event>> GetMostPopularAsync(
            int count,
            CancellationToken token);

        /// <summary>
        /// Gets event by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <exception cref="InvalidOperationException"
        /// <returns></returns>
        Task<Event> GetEventAsync(
            Guid id,
            CancellationToken token);

        Task<PaginatedEventsDto> GetPaginatedEventsAsync(
            string? title,
            DateTime? startAt,
            DateTime? endAt,
            Pagination pagination,
            CancellationToken token);
    }
}
