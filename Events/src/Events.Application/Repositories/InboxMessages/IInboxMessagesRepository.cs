using Shared.Messaging;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Application.Repositories.InboxMessages
{
    public interface IInboxMessagesRepository
    {
        Task<bool> FindMessageAsync(
            Guid messageId,
            CancellationToken cancellationToken);

        Task AddMessageAsync(
            PendingBooking message,
            CancellationToken cancellationToken);
    }
}
