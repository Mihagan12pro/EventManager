using EventManager.Domain.Bookings.Enums;
using EventManager.Domain.Events;
using System.Text.Json.Serialization;

namespace EventManager.Domain.Bookings
{
    public class BookingModel
    {
        public Guid Id { get; set; }

        public required Guid EventId { get; set; }

        public required DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required BookingStatus Status { get; set; }

        public DateTime? ProcessedAt { get; set; }

        public EventModel Event { get; set;  }

        public void Confirm()
            => Status = BookingStatus.Confirmed;

        public void Reject() =>
            Status = BookingStatus.Rejected;
    }
}
