using EventManager.Application.Handlers;
using EventManager.Application.Handlers.Bookings.Cancel;
using EventManager.Application.Handlers.Bookings.GetByIdBooking;
using EventManager.Domain.Entities.Users.Enums;
using EventManager.DTOs.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
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


        [HttpDelete("id")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] Guid id, 
            [FromServices] ICommandHandler<CancelBookingCommand> handler,
            CancellationToken cancellationToken)
        {
            await handler.HandleAsync(new CancelBookingCommand(id), cancellationToken);

            return NoContent();
        }
    }
}
