using EventManager.Domain.Bookings.Enums;
using EventManager.Domain.Events;

namespace EventManager.Domain.Bookings
{
    public class BookingModel
    {
        public Guid Id { get; set; }

        public required Guid EventId { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required BookingStatus Status { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public EventModel Event { get; set;  }

        public void Confirm()
            => Status = BookingStatus.Confirmed;

        public void Reject() =>
            Status = BookingStatus.Rejected;
    }
}
