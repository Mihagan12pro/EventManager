namespace Shared.Messaging.Contracts.Bookings
{
    public record CancelledBooking : BookingWithStatus
    {
        public CancelledBooking(string Id, string EventId, string BookingId, string OccuredAt) : base(Id, EventId, BookingId, OccuredAt)
        {
        }
    }
}
