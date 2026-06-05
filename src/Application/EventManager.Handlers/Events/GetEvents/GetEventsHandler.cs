using EventManager.Domain.Events;
using EventManager.DTOs.Events;
using EventManager.Repositories.Events;
using EventManager.Services.Exceptions.WebApi.Client.BadRequest;
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

            List<Expression<Func<EventModel, bool>>> filters = new List<Expression<Func<EventModel, bool>>>();

            if (title != null)
            {
                Expression<Func<EventModel, bool>> titleFilter = (EventModel e) => e.Title.StartsWith(title);
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

        public GetEventsHandler(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
