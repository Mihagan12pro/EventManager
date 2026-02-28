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
    }
}
