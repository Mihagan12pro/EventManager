using EventManager.Domain.Bookings.Enums;

namespace EventManager.DTOs.Bookings
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Id">Booking id. Required field</param>
    /// <param name="EventId">Accepted event id. Required field</param>
    /// <param name="Status">Status of booking. Required field</param>
    public record BookingAcceptedDto(
            Guid Id,
            Guid EventId,
            BookingStatus Status
        );
}
