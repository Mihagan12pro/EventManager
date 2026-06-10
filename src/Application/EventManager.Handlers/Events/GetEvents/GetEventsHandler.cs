using EventManager.DTOs.Events;
using EventManager.Repositories.Events;
using EventsManager.Shared.Filters;

namespace EventManager.Handlers.Events.GetEvents
{
    public class GetEventsHandler : ICommandHandler<PaginatedEventsDto, GetEventsCommand>
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<PaginatedEventsDto> HandleAsync(
            GetEventsCommand command, 
            CancellationToken cancellationToken)
        {
            var filters = new EventsFilters();
            filters.Add(command.EventsFiltersDto);

            return await _eventsRepository.GetPaginatedEventsAsync(filters, command.EventsFiltersDto.Pagination, cancellationToken);
        }

        public GetEventsHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
