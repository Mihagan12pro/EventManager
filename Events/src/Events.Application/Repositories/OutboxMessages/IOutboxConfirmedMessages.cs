using Shared.Messaging.Contracts.Bookings;

namespace Events.Application.Repositories.OutboxMessages
{
    public interface IOutboxConfirmedMessagesRepository
    {
        Task AddAsync(
            ConfirmedBooking confirmedBooking,
            CancellationToken cancellationToken);

        Task<int> GetActiveCountAsync(
            Guid userId, 
            CancellationToken cancellationToken);

        Task DeleteAllAsync(
            Guid eventId, 
            CancellationToken cancellationToken);

        Task DeleteAsync(
            Guid bookingId, 
            CancellationToken cancellationToken);
    }
}
