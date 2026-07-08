using Bookings.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Bookings.Domain
{
    public class Booking
    {
        public Guid Id { get; set; }

        public Guid? EventId { get; set; }

        public Guid? UserId { get; set; }

        public required DateTime CreatedAt { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required BookingStatus Status { get; set; }

        public DateTime? ProcessedAt { get; set; }
    }
}
