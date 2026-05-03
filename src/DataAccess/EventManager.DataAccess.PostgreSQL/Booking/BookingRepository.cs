using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.Services.Bookings;
using Microsoft.EntityFrameworkCore;

namespace EventManager.DataAccess.PostgreSQL.Booking
{
    internal class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _dbContext;
        private SemaphoreSlim _semaphore;

        public BookingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<Guid> CreateNewBookingAsync(
            Guid eventId,
            CancellationToken cancellationToken)
        {
            EventModel? @event;
            BookingModel? booking;

            try
            {
                await _semaphore.WaitAsync();

                @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
                @event.TryReverseSeats();

                booking = new BookingModel()
                {
                    CreatedAt = DateTime.UtcNow,

                    Status = BookingStatus.Pending,

                    EventId = eventId
                };

                await _dbContext.Bookings.AddAsync(booking, cancellationToken);

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                _semaphore.Release();
            }

            return booking.Id;
        }

        public async Task<IEnumerable<BookingModel>> GetAllAsync(
            BookingFiltersDto bookingFiltersDto, 
            CancellationToken cancellationToken)
        {
            IQueryable<BookingModel> bookings = _dbContext.Bookings;
            if (bookingFiltersDto.Status != null)
            {
                bookings = bookings.Where(b => b.Status == bookingFiltersDto.Status);
            }

            if (bookingFiltersDto.CreatedAt != null)
            {
                bookings = bookings.Where(b => b.CreatedAt == bookingFiltersDto.CreatedAt);
            }

            if (bookingFiltersDto.ProcessedAt != null)
            {
                bookings = bookings.Where(b => b.CreatedAt == bookingFiltersDto.ProcessedAt);
            }

            return bookings;
        }

        public async Task<BookingModel> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            BookingModel booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

            return booking;
        }

        public async Task ProcessBookingAsync(
            BookingProcessedDto bookingProcessedDto, 
            CancellationToken cancellationToken)
        {
            BookingModel? booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == bookingProcessedDto.Id, cancellationToken);

            if (bookingProcessedDto.Status != BookingStatus.Pending)
            {
                booking?.Status = bookingProcessedDto.Status;
                booking?.ProcessedAt = DateTime.UtcNow;
            }
            
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
