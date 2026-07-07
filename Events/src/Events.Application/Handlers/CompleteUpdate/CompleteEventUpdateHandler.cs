using Events.Application.Repositories;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.CompleteUpdate
{
    internal class CompleteEventUpdateHandler : ICommandHandler<CompleteEventUpdateCommand>
    {
        private readonly IWriteEventsRepository _writeEventsRepository;

        public async Task HandleAsync(
            CompleteEventUpdateCommand command, 
            CancellationToken cancellationToken)
                => await _writeEventsRepository.UpdateAsync(
                    command.eventId,
                    command.UpdateEvent,
                    cancellationToken);

        public CompleteEventUpdateHandler(IWriteEventsRepository writeEventsRepository)
        {
            _writeEventsRepository = writeEventsRepository;
        }
    }
}
