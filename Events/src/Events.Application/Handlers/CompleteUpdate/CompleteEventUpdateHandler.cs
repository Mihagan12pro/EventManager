using Events.Application.Repositories.Cache;
using Events.Application.Repositories.Events;
using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.CompleteUpdate
{
    internal class CompleteEventUpdateHandler : ICommandHandler<CompleteEventUpdateCommand>
    {
        private readonly ICacheRepository _cacheRepository;

        private readonly IWriteEventsRepository _writeEventsRepository;

        public async Task HandleAsync(
            CompleteEventUpdateCommand command,
            CancellationToken cancellationToken)
        {
            await _writeEventsRepository.UpdateAsync(
                    command.EventId,
                    command.UpdateEvent,
                    cancellationToken);

            await _cacheRepository.RemoveAsync($"events:event:{command.EventId}", cancellationToken);
        }

        public CompleteEventUpdateHandler(
            ICacheRepository cacheRepository,
            IWriteEventsRepository writeEventsRepository)
        {
            _cacheRepository = cacheRepository;

            _writeEventsRepository = writeEventsRepository;
        }
    }
}
