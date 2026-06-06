using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
using EventsManager.Shared.Filters;
using System.Linq.Expressions;

namespace EventManager.Handlers.Events.GetEvents
{
    public class GetEventsHandler : ICommandHandler<PaginatedEventsDto, GetEventsCommand>
    {
        private readonly IEventsRepository _eventsRepository;

        public async Task<PaginatedEventsDto> HandleAsync(
            GetEventsCommand command, 
            CancellationToken cancellationToken)
        {
            var pagination = command.Pagination;
            var title = command.Title;
            var dateRange = command.DateRange;

            if (pagination.Page < 0 || pagination.PageSize < 0)
                throw new BadRequestException("Pagination parameters must be greater than zero!");

            EventsFilters filters = new EventsFilters();
            filters.Add((EventModel e) => e.Title.StartsWith(title), () => title != null);
            filters.Add((EventModel e) => e.StartAt == dateRange.LowerBound, () => dateRange.LowerBound != null);
            filters.Add((EventModel e) => e.EndAt == dateRange.UpperBound, () => dateRange.UpperBound != null);

            return await _eventsRepository.GetPaginatedEventsAsync(filters, pagination, cancellationToken);
        }

        public GetEventsHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
