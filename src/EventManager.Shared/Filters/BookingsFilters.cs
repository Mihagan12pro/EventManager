using EventManager.Domain.Entities.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Shared.Filters
{
    public class BookingsFilters : Filters<BookingEntity>
    {
        public void Add(GetBookingFiltersDto bookingsDto)
        {
            Add((BookingEntity b) => b.Status == bookingsDto.Status);
            Add((BookingEntity b) => b.CreatedAt == bookingsDto.CreatedAt);
            Add((BookingEntity b) => b.ProcessedAt == bookingsDto.ProcessedAt);
        }
    }
}
