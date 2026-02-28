using EventManager.DomainModels;
using EventManager.DTOs.Events;
using EventManager.Shared;

namespace EventManager.Services.Events
{
    public class EventsService : IEventsService
    {
        private static readonly List<Event> _events = new List<Event>();

        public async Task<Result> AddNew(NewEventDto request)
        {
            if (request.StartAt >= request.EndAt)
            {
                return new Result(false, "End time must be greater than start time!");
            }

            Event createdEvent = new Event() 
            {
                Id = Guid.NewGuid(),

                Title = request.Title,

                StartAt = request.StartAt,

                EndAt = request.EndAt,

                Description = request.Description
            };

            _events.Add(createdEvent);

            return new Result(true, null!);
        }
    }
}
