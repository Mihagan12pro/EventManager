using EventManager.Domain.Bookings;
using EventManager.Services.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers
{
    [ApiController]
    [Route("/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Booking booking = await _bookingService.GetBookingByIdAsync(id);

            return Ok(booking);
        }


        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
    }
}
