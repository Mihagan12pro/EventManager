using Shared.Messaging.Contracts.Bookings;

namespace Bookings.Infrastructure.Messaging.Publishers
{
    internal interface IPublisher
    {
        Task<IPublisher> ProduceAsync(
            PendingBooking pendingBooking,
            CancellationToken cancellationToken);
    }
}
