using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Handlers.Extensions.Validation;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventsManager.Failures.Errors.Collections;
using EventsManager.Shared.Filters;
using FluentValidation;

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
            var paginationValidationResult = _paginationValidator.Validate(command.EventsFiltersDto.Pagination);
            paginationValidationResult.ThrowIfNotIsValid();
            //if (!paginationValidationResult.IsValid)
            //{
            //    ErrorsCollection errors = new ErrorsCollection(paginationValidationResult.Errors.Select(vf => vf.ToError()));

            //    throw new BadRequestException(errors);
            //}
            var filters = new EventsFilters();
            filters.Add(command.EventsFiltersDto);

            return await _eventsRepository.GetPaginatedEventsAsync(filters, command.EventsFiltersDto.Pagination, cancellationToken);
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
