using Shared.Messaging;

namespace Events.Application.Repositories.InboxMessages
{
    public interface IInboxMessagesRepository<TMessage>
        where TMessage : IMessage
    {
        Task<bool> FindPendingMessageAsync(
            Guid messageId,
            CancellationToken cancellationToken);

        Task AddMessageAsync(
            TMessage message,
            CancellationToken cancellationToken);
    }
}
