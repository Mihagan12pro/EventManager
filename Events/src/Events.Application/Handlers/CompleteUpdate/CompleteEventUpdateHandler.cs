using Shared.Objects.Interfaces;

namespace Events.Application.Handlers.CompleteUpdate
{
    internal class CompleteEventUpdateHandler : ICommandHandler<CompleteEventUpdateCommand>
    {
        public async Task HandleAsync(
            CompleteEventUpdateCommand command, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public CompleteEventUpdateHandler()
        {
            
        }
    }
}
