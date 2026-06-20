using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.Domain.Entities.Events;
using EventManager.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventManager.Domain.Entities.Bookings
{
    public class BookingEntity
    {
        public Guid Id { get; set; }

        public Guid? EventId { get; set; }

        public required Guid UserId { get; set; } 

        public required DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required BookingStatus Status { get; set; }

        public DateTime? ProcessedAt { get; set; }

        [JsonIgnore]
        public EventEntity Event { get; set; } = null!;

        [JsonIgnore]
        public UserEntity User { get; set; } = null!;

        public void Confirm()
            => Status = BookingStatus.Confirmed;

        public void Reject() =>
            Status = BookingStatus.Rejected;
    }
}
