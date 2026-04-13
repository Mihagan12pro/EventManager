using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.Conflict;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;

namespace EventManager.Services.Bookings
{
    internal class BookingsService : IBookingsService
    {
        private readonly IEventsService _eventsService;
        private readonly List<Booking> _bookings = new List<Booking>();

        private readonly object _bookingLock = new();

        public async Task<BookingAcceptedDto> CreateBookingAsync(Guid eventId)
        {
            BookingAcceptedDto bookingAcceptedDto;

            try
            {
                var eventById = await _eventsService.GetEventByIdAsync(eventId);

                lock(_bookingLock)
                {
                    if (!eventById.TryReverseSeats())
                        throw new NoAvailableSeatsException();
                }

                Booking booking = new Booking()
                { 
                    CreatedAt = DateTime.UtcNow,
                    EventId = eventId,
                    Id = Guid.NewGuid(), 
                    Status = BookingStatus.Pending
                };

                _bookings.Add(booking);

                bookingAcceptedDto = new BookingAcceptedDto(
                    booking.Id,
                    eventId,
                    booking.Status
                );
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

        public async Task<IEnumerable<Booking>> GetAllAsync(BookingFiltersDto filtersDto)
        {
            var result = _bookings.Select(b => b);

            if (filtersDto.ProcessedAt != null)
                result = result.Where(b => b.ProcessedAt == filtersDto.ProcessedAt);

            if (filtersDto.CreatedAt != null)
                result = result.Where(b => b.CreatedAt == filtersDto.CreatedAt);

            if (filtersDto.Status != null)
                result = result.Where(b => b.Status == filtersDto.Status);

            return result.ToArray();
        }

        public async Task<IEnumerable<Booking>> GetPendingAsync()
        {
            return _bookings.Where(b => b.Status == BookingStatus.Pending);
        }

        public BookingsService(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }
    }
}
