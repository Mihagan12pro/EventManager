using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using EventManager.Services.Extensions.Validation;
using EventsManager.Failures.Errors.Collections;
using FluentValidation;
using System.Linq.Expressions;

namespace EventManager.Services.Events
{
    internal class EventsService : IEventsService
    {
        private readonly IEventsRepository _eventsRepository;

        private readonly IValidator<NewEventDto> _newEventValidator;
        private readonly IValidator<PutEventDto> _putEventValidator;

        public async Task<Guid> AddNewAsync(NewEventDto request, CancellationToken cancellationToken)
        {
            var validation = _newEventValidator.Validate(request);

            if (!validation.IsValid)
            {
                ErrorsCollection errors = new ErrorsCollection(validation.Errors.Select(vf => vf.ToError()));

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

            List<Expression<Func<EventModel, bool>>> filters = new List<Expression<Func<EventModel, bool>>>();
            
            if (title != null)
            {
                Expression<Func<EventModel, bool>>  titleFilter = (EventModel e) => e.Title.StartsWith(title); 
                filters.Add(titleFilter);
            }
            
            if (dateRange.LowerBound != null)
            {
                Expression<Func<EventModel, bool>> lowerBoundFilter = (EventModel e) => e.StartAt == dateRange.LowerBound;
                filters.Add(lowerBoundFilter);
            }

            if (dateRange.UpperBound != null)
            {
                Expression<Func<EventModel, bool>> upperBoundFilter = (EventModel e) => e.EndAt == dateRange.UpperBound;
                filters.Add(upperBoundFilter);
            }


            return await _eventsRepository.GetPaginatedEventsAsync(filters, pagination, cancellationToken);
        }

        public async Task<string> UpdateByPutAsync(
            Guid id,
            PutEventDto putEvent,
            CancellationToken cancellationToken)
        {
            EventModel? eventById = await _eventsRepository.GetByIdAsync(id, cancellationToken);

            if (eventById == null)
                throw new NotFoundException($"Event with id = '{id}' was not found!");

            var validation = _putEventValidator.Validate(putEvent);

            if (!validation.IsValid)
            {
                ErrorsCollection errors = new ErrorsCollection(validation.Errors.Select(vf => vf.ToError()));

                throw new BadRequestException(errors);
            }

            await _eventsRepository.CompleteUpdateAsync(id, putEvent, cancellationToken);

            return $"Event with id = {id} had been updated!";
        }


        public EventsService(
            IEventsRepository eventsRepository, 
            IValidator<NewEventDto> newEventValidator,
            IValidator<PutEventDto> putEventValidator)
        {
            _eventsRepository = eventsRepository;

            _newEventValidator = newEventValidator;
            _putEventValidator = putEventValidator;
        }
    }
}
