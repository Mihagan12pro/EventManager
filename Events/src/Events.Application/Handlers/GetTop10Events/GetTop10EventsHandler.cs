using Events.Application.Dtos;
using Events.Application.Repositories.Events;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.GetTop10Events
{
    internal class GetTop10EventsHandler : ICommandHandler<IEnumerable<GetEventDto>, GetTop10EventsCommand>
    {
        private readonly IReadEventsRepository _eventsRepository;

        public async Task<IEnumerable<GetEventDto>> HandleAsync(
            GetTop10EventsCommand command,
            CancellationToken cancellationToken)
        {
            var events = await _eventsRepository.GetMostPopularAsync(10, cancellationToken);

            return events.Select(e => new GetEventDto(
                e.Id, 
                e.Title, 
                e.StartAt,
                e.EndAt,
                e.Description,
                e.TotalSeats,
                e.AvailableSeats)
            );
        }

        public GetTop10EventsHandler(IReadEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
        }
    }
}
