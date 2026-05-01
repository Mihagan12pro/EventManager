using CSharpFunctionalExtensions;
using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using Shared;

namespace EventManager.Services.Events
{
    internal class InMemoryEventsService : IEventsService
    {
        private readonly List<EventModel> _events = new List<EventModel>();

        public async Task<Guid> AddNewAsync(NewEventDto request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;

            if (!request.StartAt.HasValue || !request.EndAt.HasValue)
                throw new BadRequestException("Start date time and End date time are required fields!");

            DateTime start = request.StartAt.Value;
            DateTime end = request.EndAt.Value;
            int totalSeats = request.TotalSeats.Value;

            DateSpan startSpan = new DateSpan(start, now);
            DateSpan endSpan = new DateSpan(end, now);

            if (startSpan.Day <= 0 && startSpan.Year <= 0 && startSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (endSpan.Day <= 0 && endSpan.Year <= 0 && endSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (totalSeats < 1)
                throw new BadRequestException("Count of total seats must be greater than zero!");

            if (start >= end)
                throw new BadRequestException("Start date time must be greater than end date time!");


            EventModel createdEvent = new EventModel()
            {
                Id = Guid.NewGuid(),

                Title = request.Title,

                StartAt = start,

                EndAt = end,

                TotalSeats = totalSeats,

                AvailableSeats = totalSeats,

                Description = request.Description
            };

            _events.Add(createdEvent);

            return createdEvent.Id;
        }

        public async Task<string> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            EventModel? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
                throw new NotFoundException($"Event with id = '{id}' was not found!");

            _events.Remove(eventById);

            return "Event had been deleted!";
        }

        public async Task<EventModel> GetEventByIdAsync(Guid id)
        {
            EventModel? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
                throw new NotFoundException($"Event with id = '{id}' was not found!");

            return eventById;
        }

        public Task<EventModel> GetEventByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginatedEventsDto> GetEventsAsync(
            string? title,
            PaginationDto pagination,
            DateRange dateRange,
            CancellationToken cancellationToken)
        {
            if (pagination.Page < 0 || pagination.PageSize < 0)
                throw new BadRequestException("Pagination parameters must be greater than zero!");

            IEnumerable<EventModel> filteredEvents = _events;

            if (dateRange.UpperBound.HasValue || dateRange.LowerBound.HasValue)
                filteredEvents = filteredEvents.Where(e => dateRange.CheckDateRange(e).IsSuccess);

            if (title != null)
                filteredEvents = filteredEvents.Where(e => e.Title.ToLower().Contains(title.ToLower()));

            int totalCount = filteredEvents.Count();


            var eventsList = PaginationMaster<EventModel>.DoPagination(filteredEvents, pagination)
                .ToList();

            PaginatedEventsDto result = new PaginatedEventsDto(
                totalCount, 
                eventsList.AsReadOnly(),
                pagination.Page,
                pagination.PageSize);

            return result;
        }

        public async Task<string> UpdateByPutAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;

            DateTime start = putEvent.StartAt!.Value;
            DateTime end = putEvent.EndAt!.Value;

            DateSpan startSpan = new DateSpan(start, now);
            DateSpan endSpan = new DateSpan(end, now);

            EventModel? eventById = _events.FirstOrDefault(e => e.Id == id);

            if (eventById == null)
                throw new NotFoundException($"Event with id = '{id}' was not found!");

            if (startSpan.Day <= 0 && startSpan.Year <= 0 && startSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (endSpan.Day <= 0 && endSpan.Year <= 0 && endSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (putEvent.StartAt >= putEvent.EndAt)
                throw new BadRequestException("End time must be greater than start time!");

            eventById.StartAt = putEvent.StartAt!.Value;
            eventById.EndAt = putEvent.EndAt!.Value;
            eventById.Title = putEvent.Title;
            eventById.Description = putEvent.Description;

            return "Event had been updated!";
        }
    }
}
