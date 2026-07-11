namespace Shared.Messaging.Contracts.Bookings
{
    public class BookingWithStatus : IMessage
    {
        /// <summary>
        /// Message Id
        /// </summary>
        public required string Id { get; set; }

        public required string EventId { get; set; }

        public required string BookingId { get; set; }

        public required string OccurredAt { get; set; }
    }
}