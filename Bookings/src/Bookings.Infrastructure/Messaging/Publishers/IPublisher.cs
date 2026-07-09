using Shared.Messaging.Contracts.Bookings;

namespace Bookings.Infrastructure.Messaging.Publishers
{
    internal interface IPublisher
    {
        Task ProduceAsync(
            PendingBooking pendingBooking,
            CancellationToken cancellationToken);
    }
}
