using EventManager.Domain.Bookings.Enums;

namespace EventManager.DTOs.Bookings
{
    public record BookingAcceptedDto(
            Guid Id,
            Guid EventId,
            BookingStatus Status
        );
}
