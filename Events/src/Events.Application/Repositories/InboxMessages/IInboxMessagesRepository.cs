using Shared.Messaging;

namespace Events.Application.Repositories.InboxMessages
{
    public interface IInboxMessagesRepository<TMessage>
        where TMessage : IMessage
    {
        Task<bool> FindMessageAsync(
            TMessage message,
            CancellationToken cancellationToken);

        Task AddMessageAsync(
            TMessage message,
            CancellationToken cancellationToken);
    }
}
