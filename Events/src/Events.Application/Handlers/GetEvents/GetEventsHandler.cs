using Events.Application.Dtos;
using Events.Application.Repositories.Events;
using Events.Domain;
using Shared.Objects.Classes.Collections;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.GetEventsCommand
{
    public class GetEventsHandler : ICommandHandler<PaginatedEventsDto, GetEventsCommand>
    {
        private readonly IReadEventsRepository _readEventsRepository;

        public async Task<PaginatedEventsDto> HandleAsync(
            GetEventsCommand command, 
            CancellationToken cancellationToken)
        {
            Filters<Event> filters = new Filters<Event>();

            if (command.Title != null && command.Title != string.Empty) 
            {
                filters.Add((Event e) => e.Title.StartsWith(command.Title));
            }

            if (command.From != null)
            {
                filters.Add((Event e) => e.StartAt >= command.From);
            }

            if (command.To != null)
            {
                filters.Add((Event e) => e.EndAt >= command.To);
            }

            var result = await _readEventsRepository.GetPaginatedEventsAsync(filters, command.Pagination, cancellationToken);

            return result;
        }

        public GetEventsHandler(IReadEventsRepository readEventsRepository)
        {
            _readEventsRepository = readEventsRepository;
        }
    }
}
