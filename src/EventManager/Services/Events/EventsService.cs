using CSharpFunctionalExtensions;
using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Exceptions;
using EventManager.Shared;

namespace EventManager.Services.Events
{
    public class EventsService : IEventsService
    {
        private readonly List<Event> _events = new List<Event>();

        public async Task<Result<Guid, string>> AddNew(NewEventDto request)
        {
            DateTime now = DateTime.Now;

            if (!request.StartAt.HasValue || !request.EndAt.HasValue)
                throw new BadRequestException("Start date time and End date time are required fields!");

            DateTime start = request.StartAt.Value;
            DateTime end = request.EndAt.Value;

            DateSpan startSpan = new DateSpan(start, now);
            DateSpan endSpan = new DateSpan(end, now);


            if (startSpan.Day <= 0 && startSpan.Year <= 0 && startSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (endSpan.Day <= 0 && endSpan.Year <= 0 && endSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (start >= end)
                return "Start date time must be greater than end date time!";

            Event createdEvent = new Event()
            {
                Id = Guid.NewGuid(),

                Title = request.Title,

                StartAt = start,

                EndAt = end,

                Description = request.Description
            };

            _events.Add(createdEvent);

            return createdEvent.Id;
        }

        public async Task<Result<string, Error>> Delete(Guid id)
        {
            Event? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
                return Error.CreateError404($"Event with id = '{id}' was not found!");

            _events.Remove(eventById);

            return "Event had been deleted!";
        }

        public async Task<Result<GetEventDto, string>> GetEventById(Guid id)
        {
            Event? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
            {
                return $"Event with id = '{id}' was not found!";
            }

            GetEventDto eventDto = new GetEventDto(
                eventById.Title,
                eventById.StartAt, 
                eventById.EndAt,
                eventById.Description);

            return eventDto;
        }

        public async Task<PaginatedEventsDto> GetEvents(
            string? title,
            PaginationDto pagination,
            DateRange dateRange)
        {
            if (pagination.Page < 0 || pagination.PageSize < 0)
                throw new BadRequestException("Pagination parameters must be greater than zero!");

            IEnumerable<Event> filteredEvents = _events;

            if (dateRange.UpperBound.HasValue || dateRange.LowerBound.HasValue)
                filteredEvents = filteredEvents.Where(e => dateRange
                    .CheckDateRange(e)
                        .IsSuccess);

            var a = filteredEvents.ToArray().Length;

            if (title != null)
                filteredEvents = filteredEvents.Where(e => e.Title.ToLower() == title.ToLower());

            int totalCount = filteredEvents.Count();


            var eventsList = PaginationMaster<Event>.DoPagination(filteredEvents, pagination)
                .ToList();

            PaginatedEventsDto result = new PaginatedEventsDto(
                totalCount, 
                eventsList.AsReadOnly(),
                pagination.Page,
                pagination.PageSize);

            return result;
        }

        public async Task<IReadOnlyCollection<Event>> GetEvents()
        {
            return _events.AsReadOnly();
        }

        public async Task<Result<string, Error>> UpdateByPut(Guid id, NewEventDto putEvent)
        {
            Event? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
                return Error.CreateError404($"Event with id = '{id}' was not found!");

            if (putEvent.StartAt >= putEvent.EndAt)
                return Error.CreateError400("End time must be greater than start time!");

            eventById.StartAt = putEvent.StartAt!.Value;
            eventById.EndAt = putEvent.EndAt!.Value;
            eventById.Title = putEvent.Title;
            eventById.Description = putEvent.Description;

            return "Event had been updated!";
        }
    }
}
