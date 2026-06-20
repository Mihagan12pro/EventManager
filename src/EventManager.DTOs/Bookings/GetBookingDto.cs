using EventManager.Domain.Entities.Bookings.Enums;

namespace EventManager.DTOs.Bookings
{
    /// <summary>
    /// Use for getting bookings instead of using domain model
    /// </summary>
    /// <param name="EventId"></param>
    /// <param name="CreatedAt"></param>
    /// <param name="ProcessedAt"></param>
    /// <param name="Status"></param>
    public record GetBookingDto(
        Guid? EventId, 
        DateTime CreatedAt,
        DateTime? ProcessedAt,
        BookingStatus Status);
}
