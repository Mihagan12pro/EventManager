using EventManager.DomainModels;
using EventManager.DTOs.Events;
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
        public Task<Result> AddNew(NewEventDto request);

        /// <summary>
        /// Returns all Events from database
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Event>> GetEvents();

        /// <summary>
        /// Returns event by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Result> GetEventById(Guid id);

        /// <summary>
        /// Deletes event from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Result> Delete(Guid id);

        /// <summary>
        /// Updates every field of event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="putEvent"></param>
        /// <returns></returns>
        public Task<(Result, int)> UpdateByPut(Guid id, NewEventDto putEvent);
    }
}
