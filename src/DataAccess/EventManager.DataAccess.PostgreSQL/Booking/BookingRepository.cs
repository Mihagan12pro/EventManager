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

        public BookingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateNewBookingAsync(
            Guid eventId,
            CancellationToken cancellationToken)
        {
            EventModel? @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
            @event.TryReleaseSeats();

            BookingModel booking = new BookingModel()
            { 
                CreatedAt = DateTime.Now,
                
                Status = BookingStatus.Pending,

                EventId = eventId
            };

            await _dbContext.Bookings.AddAsync(booking, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }

        public async Task<IReadOnlyCollection<BookingModel>> GetAllAsync(
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

            List<BookingModel> inMemoryBooking = await bookings.ToListAsync();

            return inMemoryBooking.AsReadOnly();
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
                booking?.CreatedAt = DateTime.Now;
            }
            
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
