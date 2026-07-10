namespace Shared.Messaging.Contracts.Bookings
{
    public record BookingWithStatus : IMessage
    {
        /// <summary>
        /// Message Id
        /// </summary>
        public string Id { get; init; }

        public string EventId { get; init; }

        public string BookingId { get; init; }

        public string OccurredAt { get; init; }

        public BookingWithStatus(
            string Id,
            string EventId,
            string BookingId,
            string OccuredAt)
        {
            this.Id = Id;

            this.BookingId = BookingId;

            this.OccurredAt = OccuredAt;

            this.EventId = EventId;
        }
    }
}