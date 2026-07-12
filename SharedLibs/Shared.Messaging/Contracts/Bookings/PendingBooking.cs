namespace Shared.Messaging.Contracts.Bookings
{
    public class PendingBooking : IMessage
    {
        /// <summary>
        /// Message Id
        /// </summary>
        public required Guid Id { get; set; }

        public required Guid EventId { get; set; }

        public required Guid BookingId { get; set; }

        public required DateTime OccurredAt { get; set; }
    }
}
