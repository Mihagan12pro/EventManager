using Shared.Messaging.Contracts.Bookings;

namespace Bookings.Application.Publishers
{
    public interface IPublisher
    {
        Task ProduceAsync(
            CancelledBooking pendingBooking,
            CancellationToken cancellationToken);

        Task ProduceAsync(
            PendingBooking cancelledBooking,
            CancellationToken cancellationToken);
    }
}
