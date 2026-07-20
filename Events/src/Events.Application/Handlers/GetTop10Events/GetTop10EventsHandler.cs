using Events.Application.Dtos;
using Events.Application.Repositories.Cache;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.GetTop10Events
{
    internal class GetTop10EventsHandler : ICommandHandler<IEnumerable<GetEventDto>, GetTop10EventsCommand>
    {
        private readonly ICacheRepository _cacheRepository;

        public async Task<IEnumerable<GetEventDto>> HandleAsync(
            GetTop10EventsCommand command,
            CancellationToken cancellationToken)
        {
            var events = await _cacheRepository.GetMostPopularAsync(10, cancellationToken);

            if (events == null)
                return null;

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

        public GetTop10EventsHandler(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }
    }
}
