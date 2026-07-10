using Shared.Messaging;

namespace Events.Application.Repositories.InboxMessages
{
    public interface InboxMessagesRepository
    {
        Task<bool> FindMessageAsync(
            Guid messageId,
            CancellationToken cancellationToken);

        Task AddMessageAsync(
            Message message,
            CancellationToken cancellationToken);
    }
}
