using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Extensions;
using EventManager.Services.Events;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers
{
    [ApiController]
    [Route("/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventService;

        [HttpPost]
        //[ApiVersion("v2")]
        public async Task<IActionResult> New([FromBody] NewEventDto newEvent)
        {
            var result = await _eventService.AddNew(newEvent);

            if (result.IsSuccess)
            {
                var output = result.Value;
                var request = HttpContext.Request;

                string uri = $"{request.Scheme}://{request.Host}{request.Path}/{output}";
                return Created(uri, output);
            }

            return BadRequest(result.Error);
        }

        [HttpGet]
        //[ApiVersion("v2")]
        public async Task<IActionResult> All(
            [FromQuery] string? title,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            PaginationDto pagination = new PaginationDto(page, limit);

            DateRange dateRange = new DateRange(
                from,
                false, 
                to,
                false);

            var events = await _eventService.GetEvents(title, pagination, dateRange);

            return Ok(events);
        }

        [HttpGet("{id}")]
        //[ApiVersion("v2")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await _eventService.GetEventById(id);

            if (result.IsSuccess)
                return Ok(result.Value);

            return NotFound(result.Error);
        }

        [HttpDelete("{id}")]
        //[ApiVersion("v2")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _eventService.Delete(id);

            if (result.IsSuccess)
                return Ok(result.Value);

            return NotFound(result.Error.Message);
        }

        [HttpPut("{id}")]
        //[ApiVersion("v2")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] NewEventDto newEvent)
        {
            var result = await _eventService.UpdateByPut(id, newEvent);


            if (result.IsFailure)
                return this.ErrorToActionResult(result.Error);

            return Ok(result.Value);
        }

        public EventsController(IEventsService eventsService)
        {
            _eventService = eventsService;
        }
    }
}
