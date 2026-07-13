using Events.Application.Repositories.Events;
using Events.Domain;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Contracts.Events;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.Cancel
{
    internal class CancelEventHandler : ICommandHandler<CancelEventCommand>
    {
        private readonly IPublisher _publisher;
        private readonly IWriteEventsRepository _writeEventsRepository;
        private readonly IReadEventsRepository _readEventsRepository;
        private readonly ILogger<CancelEventHandler> _logger;

        public async Task HandleAsync(
            CancelEventCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                Event @event = await _readEventsRepository.GetEventAsync(command.Id, cancellationToken);

                DateTime now = DateTime.UtcNow;

                if (now < @event.StartAt)
                {
                    await _writeEventsRepository.DeleteAsync(command.Id, cancellationToken);

                    await _publisher.PublishEventDeletedAsync(
                        new DeletedEvent()
                        {
                            EventId = @event.Id
                        },

                        cancellationToken
                    );
                }
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogInformation("Event with id = {id} doest not exists!", command.Id);
            }
        }

        public CancelEventHandler(
            ILogger<CancelEventHandler> logger,
            IPublisher publisher,
            IWriteEventsRepository writeEventsRepository,
            IReadEventsRepository readEventsRepository)
        {
            _logger = logger;

            _publisher = publisher;

            _writeEventsRepository = writeEventsRepository;

            _readEventsRepository = readEventsRepository;
        }
    }
}
