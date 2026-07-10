using Events.Application.Repositories.Events;
using Shared.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Application.Handlers.Cancel
{
    internal class CancelEventHandler : ICommandHandler<CancelEventCommand>
    {
        private readonly IWriteEventsRepository _writeEventsRepository;

        public async Task HandleAsync(
            CancelEventCommand command,
            CancellationToken cancellationToken)
                => await _writeEventsRepository.DeleteAsync(command.Id, cancellationToken);

        public CancelEventHandler(IWriteEventsRepository writeEventsRepository)
        {
            _writeEventsRepository = writeEventsRepository;
        }
    }
}
