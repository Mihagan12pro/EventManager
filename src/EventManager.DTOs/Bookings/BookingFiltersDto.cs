using EventManager.Domain.Bookings.Enums;

namespace EventManager.DTOs.Bookings
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Status">Booking status. Optional field</param>
    /// <param name="CreatedAt">Date time of creating. Optional field</param>
    /// <param name="ProcessedAt">Date time of processing. Optional field</param>
    public record BookingFiltersDto(
        BookingStatus? Status,
        DateTime? CreatedAt,
        DateTime? ProcessedAt);
}
