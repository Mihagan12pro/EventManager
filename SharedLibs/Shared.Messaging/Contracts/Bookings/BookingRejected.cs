namespace Shared.Messaging.Contracts.Bookings
{
    public record BookingRejected : BookingWithStatus
    {
        public BookingRejected(
            string Id, 
            string EventId,
            string BookingId, 
            string OccuredAt) : base(Id, EventId, BookingId, OccuredAt)
        {
        }
    }
}
