using EventManager.DTOs.Bookings;

namespace EventManager.Handlers.Bookings.GetAllBookings
{
    public record GetAllBookingsCommand(BookingFiltersDto FiltersDto) : ICommand;
}
