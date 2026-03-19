using CSharpFunctionalExtensions;
using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Shared;

namespace EventManager.Services.Events
{
    public interface IEventsService
    {
        /// <summary>
        /// Creates new event
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<Result<Guid, string>> AddNew(NewEventDto request);

        /// <summary>
        /// Returns all Events from database
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public Task<IReadOnlyCollection<Event>> GetEvents();

        /// <summary>
        /// Returns all Events from database with filters
        /// </summary>
        /// <param name="title"></param>
        /// <param name="dateRange"></param>
        /// <returns></returns>
        public Task<PaginatedEventsDto> GetEvents(
            string? title,
            PaginationDto pagination,
            DateRange dateRange);

        /// <summary>
        /// Returns event by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Result<GetEventDto, string>> GetEventById(Guid id);

        /// <summary>
        /// Deletes event from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Result<string, Error>> Delete(Guid id);

        /// <summary>
        /// Updates every field of event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="putEvent"></param>
        /// <returns></returns>
        public Task<Result<string, Error>> UpdateByPut(Guid id, NewEventDto putEvent);
    }
}
