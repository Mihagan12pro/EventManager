using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Handlers.Extensions.Validation;
using EventManager.Repositories.Events;
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
            _paginationValidator.Validate(command.EventsFiltersDto.Pagination).ThrowBadRequestIfNotIsValid(); ;

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
