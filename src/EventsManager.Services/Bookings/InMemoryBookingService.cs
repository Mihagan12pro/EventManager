using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.Conflict;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;

namespace EventManager.Services.Bookings
{
    internal class InMemoryBookingsService : IBookingsService
    {
        private readonly IEventsService _eventsService;

        private readonly List<BookingModel> _bookings = new List<BookingModel>();

        private readonly object _bookingLock = new();

        public async Task<BookingAcceptedDto> CreateBookingAsync(Guid eventId, CancellationToken cancellationToken)
        {
            BookingAcceptedDto bookingAcceptedDto;

            try
            {
                var eventById = await _eventsService.GetEventByIdAsync(eventId, cancellationToken);

                BookingModel booking = new BookingModel()
                { 
                    CreatedAt = DateTime.Now,
                    EventId = eventId,
                    Id = Guid.NewGuid(), 
                    Status = BookingStatus.Pending
                };
                
                lock(_bookingLock)
                {
                    if (!eventById.TryReverseSeats())
                        throw new NoAvailableSeatsException();
                    
                    _bookings.Add(booking);
                }

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

        public async Task<BookingModel> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            BookingModel? booking = _bookings.FirstOrDefault(b => b.Id == bookingId);

            if (booking == null)
                throw new NotFoundException($"Booking with id = {bookingId} was not found!");

            return booking;
        }

        public async Task<IEnumerable<BookingModel>> GetAllAsync(BookingFiltersDto filtersDto, CancellationToken cancellationToken)
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

        public InMemoryBookingsService(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }
    }
}
