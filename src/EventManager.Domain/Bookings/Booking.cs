using EventManager.Domain.Bookings.Enums;
using System.Text.Json.Serialization;

namespace EventManager.Domain.Bookings
{
    public class Booking
    {
        public required Guid Id { get; set; }

        public required Guid EventId { get; set; }

        public required DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required BookingStatus Status { get; set; }

        public DateTime? ProcessedAt { get; set; }
    }
}
