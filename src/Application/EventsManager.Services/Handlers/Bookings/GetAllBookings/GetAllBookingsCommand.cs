using EventManager.Application;
using EventManager.DTOs.Bookings;

namespace EventManager.Application.Handlers.Bookings.GetAllBookings
{
    public record GetAllBookingsCommand(GetBookingFiltersDto FiltersDto) : ICommand;
}
