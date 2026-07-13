using Shared.Messaging.Contracts.Bookings;
using Shared.Messaging.Contracts.Events;

namespace Events.Application
{
    public interface IPublisher
    {
        Task PublishConfirmedAsync(
            ConfirmedBooking confirmed, 
            CancellationToken cancellationToken);

        Task PublishRejectedAsync(
            RejectedBooking rejected, 
            CancellationToken cancellationToken);

        Task PublishEventDeletedAsync(
            DeletedEvent deleted,
            CancellationToken cancellationToken);
    }
}