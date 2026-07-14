using Bookings.Domain.Enums;

namespace Bookings.Application.Dtos
{
    public record class GetBookingDto(
        Guid Id,
        Guid EventId,
        DateTime? ProcessedAt,
        BookingStatus Status);
}
