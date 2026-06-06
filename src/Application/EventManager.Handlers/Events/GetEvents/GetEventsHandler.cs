using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Handlers.Extensions;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventsManager.Failures.Errors.Collections;
using EventsManager.Shared.Filters;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace EventManager.Handlers.Events.GetEvents
{
    public class GetEventsHandler : ICommandHandler<PaginatedEventsDto, GetEventsCommand>
    {
        private readonly IValidator<PaginationDto> _paginationValidator;
        private readonly IEventsRepository _eventsRepository;

        public async Task<PaginatedEventsDto> HandleAsync(
            GetEventsCommand command, 
            CancellationToken cancellationToken)
        {
            var pagination = command.Pagination;
            var title = command.Title;
            var dateRange = command.DateRange;

            var paginationValidationResult = _paginationValidator.Validate(command.Pagination);
            if (!paginationValidationResult.IsValid)
            {
                ErrorsCollection errors = new ErrorsCollection(paginationValidationResult.Errors.Select(vf => vf.ToError()));

                throw new BadRequestException(errors);
            }

            Filters<EventModel> filters = new Filters<EventModel>(
                
                );
            filters.Add((EventModel e) => e.Title.StartsWith(title), () => title != null);
            filters.Add((EventModel e) => e.StartAt == dateRange.LowerBound, () => dateRange.LowerBound != null);
            filters.Add((EventModel e) => e.EndAt == dateRange.UpperBound, () => dateRange.UpperBound != null);

            return await _eventsRepository.GetPaginatedEventsAsync(filters, pagination, cancellationToken);
        }

        public GetEventsHandler(
            IEventsRepository eventsRepository,
            IValidator<PaginationDto> paginationValidator)
        {
            _eventsRepository = eventsRepository;
            _paginationValidator = paginationValidator;
        }
    }
}
