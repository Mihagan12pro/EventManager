namespace Shared.Messaging.Contracts
{
    public class PendingBooking : IMessage
    {
        /// <summary>
        /// Message Id
        /// </summary>
        public required string Id { get; set; }

        public required string EventId { get; set; }

        public required string BookingId { get; set; }
    }
}
