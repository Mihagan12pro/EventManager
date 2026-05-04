using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using EventManager.Services.Extensions.Validation;
using EventsManager.Failures.Errors.Collections;
using FluentValidation;
using Shared;

namespace EventManager.Services.Events
{
    internal class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;

        private readonly IValidator<NewEventDto> _newEventValidator;

        public async Task<Guid> AddNewAsync(NewEventDto request, CancellationToken cancellationToken)
        {
            DateTime now = DateTime.Now;

            if (!request.StartAt.HasValue || !request.EndAt.HasValue)
                throw new BadRequestException("Start date time and End date time are required fields!");

            DateTime start = request.StartAt.Value;
            DateTime end = request.EndAt.Value;
            int totalSeats = request.TotalSeats.Value;

            var validation = _newEventValidator.Validate(request);

            if (!validation.IsValid)
            {
                ErrorsCollection errors = new (validation.Errors.Select(vf => vf.ToError()));

                throw new BadRequestException(errors);
            }

            Guid id = await _eventsRepository.AddNewAsync(request, cancellationToken);

            return id;
        }

        public async Task<string> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            EventModel? @event = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (@event == null)
                throw new NotFoundException($"Event with id = {id} does not exists!");

            await _eventsRepository.DeleteAsync(id, cancellationToken);

            return $"Event with id = {id} had been deleted!";
        }

        public async Task<GetEventDto> GetEventByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            EventModel? @event = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (@event == null)
                throw new NotFoundException($"Event with id = {id} does not exists!");

            return new GetEventDto(
                @event.Id,
                @event.Title,
                @event.StartAt,
                @event.EndAt,
                @event.Description, 
                @event.TotalSeats,
                @event.AvailableSeats
            );
        }

        public async Task<PaginatedEventsDto> GetEventsAsync(
            string? title,
            PaginationDto pagination,
            DateRange dateRange, 
            CancellationToken cancellationToken)
        {
            if (pagination.Page < 0 || pagination.PageSize < 0)
                throw new BadRequestException("Pagination parameters must be greater than zero!");

            GetEventsWithFiltersDto eventsWithFiltersDto = new GetEventsWithFiltersDto(title, pagination, dateRange);

            return await _eventsRepository.GetPaginatedEventsAsync(eventsWithFiltersDto, cancellationToken);
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

            EventModel? eventById = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (eventById == null)
                throw new NotFoundException($"Event with id = '{id}' was not found!");

            if (startSpan.Day <= 0 && startSpan.Year <= 0 && startSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (endSpan.Day <= 0 && endSpan.Year <= 0 && endSpan.Month <= 0)
                throw new BadRequestException("Too late!");

            if (putEvent.StartAt >= putEvent.EndAt)
                throw new BadRequestException("End time must be greater than start time!");

            await _eventsRepository.CompleteUpdateAsync(id, putEvent, cancellationToken);

            return $"Event with id = {id} had been updated!";
        }


        public EventsService(
            IEventsRepository eventsRepository, 
            IValidator<NewEventDto> newEventValidator)
        {
            _eventsRepository = eventsRepository;

            _newEventValidator = newEventValidator;
        }
    }
}
