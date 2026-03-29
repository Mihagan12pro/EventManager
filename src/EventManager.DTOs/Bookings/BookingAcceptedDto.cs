namespace EventManager.DTOs.Bookings
{
    public record BookingAcceptedDto(
            Guid Id,
            string Message
        );

    public record BookingAcceptedWithUrlDto(
            Guid Id,
            string Message,
            string URL
        );
}
