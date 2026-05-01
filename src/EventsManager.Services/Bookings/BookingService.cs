using EventManager.Domain.Bookings;
using EventManager.DTOs.Bookings;

namespace EventManager.Services.Bookings
{
    internal class BookingsService : IBookingsService
    {
        private readonly IBookingRepository _bookingRepository;

        private readonly SemaphoreSlim _semaphore;

        public Task<BookingAcceptedDto> CreateBookingAsync(Guid eventId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Booking>> GetAllAsync(BookingFiltersDto filtersDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Booking> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Update(Booking booking, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public BookingsService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;

            _semaphore = new SemaphoreSlim(1, 1);
        }
    }
}
