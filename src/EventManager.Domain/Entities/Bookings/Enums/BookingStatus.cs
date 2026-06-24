using System.Text.Json.Serialization;

namespace EventManager.Domain.Entities.Bookings.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BookingStatus
    {
        /// <summary>
        /// Booking had been created. It is awaiting of handling
        /// </summary>
        Pending,

        /// <summary>
        /// Booking had been confirmed
        /// </summary>
        Confirmed,

        /// <summary>
        /// Booking had been rejected
        /// </summary>
        Rejected,

        /// <summary>
        /// Booking had been cancelled
        /// </summary>
        Cancelled,
    }
}
