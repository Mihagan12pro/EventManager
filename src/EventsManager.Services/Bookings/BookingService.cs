using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.Conflict;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;

namespace EventManager.Services.Bookings
{
    internal class BookingsService : IBookingsService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEventsRepository _eventsRepository;

        private readonly SemaphoreSlim _semaphore;

        public async Task<BookingAcceptedDto> CreateBookingAsync(
            Guid eventId, 
            CancellationToken cancellationToken)
        {
            Guid? bookingId;
            BookingAcceptedDto? result = null;

            EventModel @event = await _eventsRepository.GetByIdAsync(eventId, cancellationToken);
            try
            {
                await _semaphore.WaitAsync();

                if (@event.AvailableSeats == 0)
                    throw new NoAvailableSeatsException();

                Guid id = await _bookingRepository.CreateNewBookingAsync(eventId, cancellationToken);

                result = new BookingAcceptedDto(
                    id,
                    eventId, 
                    BookingStatus.Pending);
            }
            finally
            {
                _semaphore.Release();
            }

            return result;
        }

        public async Task<IEnumerable<BookingModel>> GetAllAsync(
            BookingFiltersDto filtersDto,
            CancellationToken cancellationToken)
        {
            var result = await _bookingRepository.GetAllAsync(filtersDto, cancellationToken);

            return result;
        }

        public async Task<BookingModel> GetBookingByIdAsync(
            Guid bookingId,
            CancellationToken cancellationToken)
        {
            BookingModel booking = await _bookingRepository.GetByIdAsync(bookingId, cancellationToken);
            if (booking == null)
                throw new NotFoundException($"Booking with id = {bookingId} does not exists!");

            return booking;
        }

        public BookingsService(
            IEventsRepository eventsRepository, 
            IBookingRepository bookingRepository)
        {
            _eventsRepository = eventsRepository;
            _bookingRepository = bookingRepository;

            _semaphore = new SemaphoreSlim(1, 1);
        }
    }
}
