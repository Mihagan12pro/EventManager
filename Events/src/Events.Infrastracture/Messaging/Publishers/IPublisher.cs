using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Messaging.Publishers
{
    public interface IPublisher
    {
        Task PublishConfirmedAsync(
            ConfirmedBooking confirmed, 
            CancellationToken cancellationToken);

        Task PublishRejectedAsync(
            BookingRejected rejected, 
            CancellationToken cancellationToken);
    }
}