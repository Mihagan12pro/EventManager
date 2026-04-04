using EventManager.Domain.Bookings.Enums;

namespace EventManager.DTOs.Bookings
{
    public record BookingFiltersDto(
        BookingStatus? Status,
        DateTime? CreatedAt,
        DateTime? ProcessedAt);
}
