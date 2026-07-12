using Shared.Messaging;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Application.Repositories.InboxMessages
{
    public interface IInboxMessagesRepository
    {
        Task<bool> FindPendingMessageAsync(
            Guid messageId,
            CancellationToken cancellationToken);

        Task AddMessageAsync(
            PendingBooking message,
            CancellationToken cancellationToken);
    }
}
