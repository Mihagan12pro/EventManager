using EventManager.Domain.Bookings.Enums;

namespace EventManager.DTOs.Bookings
{
    public record BookingProcessedDto(
            Guid Id,
            BookingStatus Status
        );
}
