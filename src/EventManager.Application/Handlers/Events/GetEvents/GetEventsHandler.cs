using EventManager.Application.DataAccess.Repositories;
using EventManager.DTOs.Events;
using EventManager.Shared.Filters;

namespace EventManager.Application.Handlers.Events.GetEvents
{
    internal class GetEventsHandler : ICommandHandler<PaginatedEventsDto, GetEventsCommand>
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
