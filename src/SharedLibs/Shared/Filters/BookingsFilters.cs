using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventsManager.Shared.Filters
{
    public class BookingsFilters : Filters<BookingModel>
    {
        public void Add(GetBookingFiltersDto bookingsDto)
        {
            Add((BookingModel b) => b.Status == bookingsDto.Status);
            Add((BookingModel b) => b.CreatedAt == bookingsDto.CreatedAt);
            Add((BookingModel b) => b.ProcessedAt == bookingsDto.ProcessedAt);
        }
    }
}
