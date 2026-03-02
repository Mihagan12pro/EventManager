using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers
{
    [ApiController]
    [Route("/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventService;

        [HttpPost]
        public async Task<IActionResult> New([FromBody] NewEventDto newEvent)
        {
            Result result = await _eventService.AddNew(newEvent);

            if (result.IsSuccess)
                return Ok(result.Output);

            return BadRequest(result.Output);
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            var events = await _eventService.GetEvents();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await _eventService.GetEventById(id);

            if (result.IsSuccess)
                return Ok(result.Output);

            return NotFound(result.Output);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _eventService.Delete(id);

            if (result.IsSuccess)
                return Ok(result.Output);

            return NotFound(result.Output);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] NewEventDto newEvent)
        {
            var codeResult = await _eventService.UpdateByPut(id, newEvent);

            Result result = codeResult.Item1;
            int code = codeResult.Item2;

            if (result.IsSuccess)
                return Ok(result.Output);

            switch(code)
            {
                case 400:
                    return BadRequest(result.Output);
                case 404:
                    return NotFound(result.Output);
                default:
                    return BadRequest();
            }
        }


        public EventsController(IEventsService eventsService)
        {
            _eventService = eventsService;
        }
    }
}
