using EventManager.DTOs.Bookings;

namespace EventManager.Handlers.Bookings.GetAllBookings
{
    public record GetAllBookingsCommand(GetBookingFiltersDto FiltersDto) : ICommand;
}
