using Events.Domain;
using Shared.Objects.Interfaces;
using Events.Domain.ValueObjects;
using Events.Application.Repositories.Events;

namespace Events.Application.Handlers.Add
{
    internal class AddEventHandler : ICommandHandler<Guid, AddEventCommand>
    {
        private readonly IWriteEventsRepository _writeEventsRepository;

        public async Task<Guid> HandleAsync(
            AddEventCommand command,
            CancellationToken cancellationToken)
        {
            Event @event = new Event()
            {
                EventDateTime = new (command.NewEvent.StartAt.Value, command.NewEvent.EndAt.Value),

                EventNaming = new(command.NewEvent.Title, command.NewEvent.Description),

                Seats = new Seats(command.NewEvent.TotalSeats.Value)
            };
            @event.Validate();

            Guid id = await _writeEventsRepository.AddAsync(@event, cancellationToken);

            return id;
        }

        public AddEventHandler(IWriteEventsRepository writeEventsRepository)
        {
            _writeEventsRepository = writeEventsRepository;
        }
    }
}
