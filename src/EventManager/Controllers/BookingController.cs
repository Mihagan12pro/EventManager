using EventManager.Services.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers
{
    [ApiController]
    [Route("/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;



        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }
    }
}
