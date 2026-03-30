using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.DTOs.Shared;
using EventManager.Extensions;
using EventManager.Queues.ApplicationTasks.Booking;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.Controllers
{
    [ApiController]
    [Route("/events")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventService;
        private readonly IBookingsService _bookingService;
        private readonly IBookingTaskQueue _bookingQueue;

        [HttpPost]
        public async Task<IActionResult> New([FromBody] NewEventDto newEvent)
        {
            var result = await _eventService.AddNewAsync(newEvent);

            var output = result;
            var request = HttpContext.Request;

            string uri = request.ToUrl(new List<object> { output });

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

            var events = await _eventService.GetEventsAsync(
                title,
                pagination,
                dateRange);


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

        [HttpPost("{id}/book")]
        public async Task<IActionResult> Book(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var bookingDto = await _bookingService.CreateBookingAsync(id);

            var bookingWithUrlDto = new BookingAcceptedWithUrlDto(
                bookingDto.Id, 
                bookingDto.Message, 
                HttpContext.Request.ToUrl(false, new List<object> { "bookings", bookingDto.Id })
            );

            BookingTask bookingTask = new BookingTask(bookingDto.Id);
            _bookingQueue.Enqueue(bookingTask);

            return Accepted(bookingWithUrlDto);
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
