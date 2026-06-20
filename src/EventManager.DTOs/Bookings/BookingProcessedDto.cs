using EventManager.Domain.Entities.Bookings.Enums;

namespace EventManager.DTOs.Bookings
{
    public record BookingProcessedDto(
            Guid Id,
            BookingStatus Status
        );
}
