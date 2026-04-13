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

        /// <summary>
        /// Allows to get booking by id
        /// </summary>
        /// <param name="id">Booking id. Required field</param>
        /// <response code="200">If everyting is ok</response>
        /// <response code="404">If book does not exists</response>
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
