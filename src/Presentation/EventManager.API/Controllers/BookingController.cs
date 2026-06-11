using EventManager.Application.Handlers;
using EventManager.Application.Handlers.Bookings.GetByIdBooking;
using EventManager.DTOs.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route("/bookings")]
    public class BookingController : ControllerBase
    {
        /// <summary>
        /// Allows to get booking by id
        /// </summary>
        /// <param name="id">Booking id. Required field</param>
        /// <response code="200">If everyting is ok</response>
        /// <response code="404">If book does not exists</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            [FromServices] ICommandHandler<GetBookingDto, GetByIdBookingCommand> handler,
            CancellationToken cancellationToken)
        {
            var booking = await handler.HandleAsync(new GetByIdBookingCommand(id), cancellationToken);

            return Ok(booking);
        }
    }
}
