using EventManager.Domain.Bookings;
using EventManager.Services.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers
{
    [ApiController]
    [Route("/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingsService _bookingService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Booking booking = await _bookingService.GetBookingByIdAsync(id);

            return Ok(booking);
        }


        public BookingController(IBookingsService bookingService)
        {
            _bookingService = bookingService;
        }
    }
}
