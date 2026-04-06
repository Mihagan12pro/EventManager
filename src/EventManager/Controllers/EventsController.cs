using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EventManager.Controllers
{
    [ApiController]
    [Route("/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventService;
        private readonly IBookingsService _bookingService;
        private readonly IBookingTaskQueue _bookingQueue;

        /// <summary>
        /// Adds new event
        /// </summary>
        /// <param name="newEvent"></param>
        [HttpPost]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status201Created)]
        public async Task<IActionResult> New([FromBody] NewEventDto newEvent)
        {
            var result = await _eventService.AddNewAsync(newEvent);

            var output = result;
            var request = HttpContext.Request;

            string uri = UrlMaster.CreateFromRequest(request);
            uri = UrlMaster.AddElementToEnd(uri, output);

            return Created(uri, output);
        }

        /// <summary>
        /// Allows to get all events with some filters and pagination
        /// </summary>
        /// <param name="title">Complete of event title or part of title. Helps to implement partial matching. Optional field</param>
        /// <param name="from">End of event. Optional field</param>
        /// <param name="to">Start of event. Optional field</param>
        /// <param name="page">Number of page. Must be greater or equal to zero. Required field</param>
        /// <param name="pageSize">Size of page. Must be greater or equal to zero. Required field</param>
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

            var events = await _eventService.GetEventsAsync(
                title,
                pagination,
                dateRange);


            return Ok(events);
        }

        /// <summary>
        /// Allows to get event by id
        /// </summary>
        /// <param name="id">Event id. Required field</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var result = await _eventService.GetEventByIdAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Allows to delete event
        /// </summary>
        /// <param name="id">Event id. Required field</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _eventService.DeleteAsync(id);

            return Ok(result);
        }

        /// <summary>
        /// Allows to create new event instead of old event
        /// </summary>
        /// <param name="id">Event id. Required field</param>
        /// <param name="newEvent">New event parameters. Required field</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] NewEventDto newEvent)
        {
            var result = await _eventService.UpdateByPutAsync(id, newEvent);

            return Ok(result);
        }

        /// <summary>
        /// Allows to book event
        /// </summary>
        /// <param name="id">Events id. Required field</param>
        /// <param name="cancellationToken"></param>
        /// <response code="202"></response>
        [HttpPost("{id}/book")]
        public async Task<IActionResult> Book(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var bookingDto = await _bookingService.CreateBookingAsync(id);

            var location = UrlMaster.CreateWithoutPath(HttpContext.Request, "bookings", bookingDto.Id);

            return Accepted(location, bookingDto);
        }

        public EventsController(
            IEventsService eventsService,
            IBookingsService bookingService,
            IBookingTaskQueue bookingQueue)
        {
            _eventService = eventsService;
            _bookingService = bookingService;
            _bookingQueue = bookingQueue;
        }
    }
}
