using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.Add
{
    internal class AddEventHandler : ICommandHandler<Guid, AddEventCommand>
    {
        public async Task<Guid> HandleAsync(
            AddEventCommand command,
            CancellationToken cancellationToken)
        {
            return Guid.NewGuid();
        }
    }
}
