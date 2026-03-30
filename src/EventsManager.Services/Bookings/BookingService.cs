using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Events;
using EventManager.Services.Exceptions;

namespace EventManager.Services.Bookings
{
    internal class BookingsService : IBookingsService
    {
        private readonly IEventsService _eventsService;
        private readonly List<Booking> _bookings = new List<Booking>();

        public async Task<BookingAcceptedDto> CreateBookingAsync(Guid eventId)
        {
            BookingAcceptedDto bookingAcceptedDto;

            try
            {
                GetEventDto eventById = await _eventsService.GetEventByIdAsync(eventId);

                Booking booking = new Booking()
                { 
                    CreatedAt = DateTime.Now,
                    EventId = eventId,
                    Id = Guid.NewGuid(), 
                    Status = BookingStatus.Pending
                };

                _bookings.Add(booking);

                bookingAcceptedDto = new BookingAcceptedDto(booking.Id, "Your request is pending!");
            }
            catch
            {
                throw;
            }

            return bookingAcceptedDto;
        }

        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            Booking? booking = _bookings.FirstOrDefault(b => b.Id == bookingId);

            if (booking == null)
                throw new NotFoundException($"Booking with id = {bookingId} was not found!");

            return booking;
        }


        public BookingsService(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }
    }
}
