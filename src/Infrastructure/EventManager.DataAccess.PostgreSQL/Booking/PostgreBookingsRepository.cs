using EventManager.Application.Repositories;
using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Shared;
using EventManager.Shared.Filters;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.PostgreSQL.Booking
{
    public class PostgreBookingsRepository : IBookingsRepository
    {
        private readonly AppDbContextBase _dbContext;
        private SemaphoreSlim _semaphore;

        public PostgreBookingsRepository(AppDbContextBase dbContext)
        {
            _dbContext = dbContext;

            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<Guid> CreateNewBookingAsync(
            Guid eventId,
            CancellationToken cancellationToken)
        {
            EventEntity? @event;
            BookingEntity? booking;

            try
            {
                await _semaphore.WaitAsync();

                @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
                NullChecker.Check(@event);

                @event.ReverseSeats();

                booking = new BookingEntity()
                {
                    CreatedAt = DateTime.UtcNow,

                    Status = BookingStatus.Pending,

                    EventId = eventId
                };

                await _dbContext.Bookings.AddAsync(booking, cancellationToken);
            }
            finally
            {
                await _dbContext.SaveChangesAsync(cancellationToken);

                _semaphore.Release();
            }

            return booking.Id;
        }

        public async Task<IEnumerable<BookingEntity>> GetAllAsync(
            Filters<BookingEntity> filters, 
            CancellationToken cancellationToken)
        {
            IQueryable<BookingEntity> bookings = _dbContext.Bookings;

            bookings = filters.ApplyFilters(bookings);

            return bookings;
        }

        public async Task<BookingEntity> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            BookingEntity booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

            return booking;
        }

        public async Task ProcessBookingAsync(
            BookingProcessedDto bookingProcessedDto, 
            CancellationToken cancellationToken)
        {
            BookingEntity booking = await _dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == bookingProcessedDto.Id, cancellationToken);

            if (bookingProcessedDto.Status != BookingStatus.Pending)
            {
                booking.Status = bookingProcessedDto.Status;
                booking.ProcessedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}