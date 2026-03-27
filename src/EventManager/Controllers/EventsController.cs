using EventManager.DomainModels.Events;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
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
        public async Task<IActionResult> New([FromBody] NewEventDto newEvent)
        {
            var result = await _eventService.AddNewAsync(newEvent);

            var output = result;
            var request = HttpContext.Request;

            string uri = $"{request.Scheme}://{request.Host}{request.Path}/{output}";
            return Created(uri, output);
        }

        [HttpGet]
        public async Task<IActionResult> All(
            [FromQuery] string? title,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            PaginationDto pagination = new PaginationDto(page, pageSize);

            DateRange dateRange = new DateRange(
                from,
                false, 
                to,
                false);

            var events = await _eventService.GetEventsAsync(title, pagination, dateRange);

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await _eventService.GetEventByIdAsync(id);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _eventService.DeleteAsync(id);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] NewEventDto newEvent)
        {
            var result = await _eventService.UpdateByPutAsync(id, newEvent);

            return Ok(result);
        }

        public EventsController(IEventsService eventsService)
        {
            _eventService = eventsService;
        }
    }
}
