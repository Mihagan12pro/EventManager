using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventService;

        [HttpPost]
        public async Task<IActionResult> New([FromBody] NewEventDto newEvent)
        {
            Result result = await _eventService.AddNew(newEvent);

            if (result.IsSuccess)
                return Ok();

            return BadRequest(result.Output);
        }

        public EventsController(IEventsService eventsService)
        {
            _eventService = eventsService;
        }
    }
}
