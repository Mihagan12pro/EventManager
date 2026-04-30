using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;

namespace EventManager.Services.Events
{
    public interface IEventsService
    {
        /// <summary>
        /// Creates new event
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<Guid> AddNewAsync(
            NewEventDto request, 
            CancellationToken cancellationToken);


        /// <summary>
        /// Returns all Events from database with filters
        /// </summary>
        /// <param name="title"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        Task<PaginatedEventsDto> GetEventsAsync(
            string? title,
            PaginationDto pagination,
            DateRange dateRange, 
            CancellationToken cancellationToken);

        /// <summary>
        /// Returns event by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Event> GetEventByIdAsync(
            Guid id, 
            CancellationToken cancellationToken);

        /// <summary>
        /// Deletes event from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken);

        /// <summary>
        /// Updates every field of event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="putEvent"></param>
        /// <returns></returns>
        Task<string> UpdateByPutAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken);
    }
}
