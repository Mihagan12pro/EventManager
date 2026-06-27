using EventManager.Application.DataAccess.Repositories;
using EventManager.Domain.Entities.Bookings;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.Domain.Entities.Events;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.Conflict;
using EventManager.DTOs.Bookings;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Shared.Filters;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Infrastructure.PostgreSQL.Booking
{
    public class PostgreBookingsRepository : IBookingsRepository
    {
        private readonly AppDbContext _dbContext;
        private SemaphoreSlim _semaphore;

        public PostgreBookingsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;

            _semaphore = new SemaphoreSlim(1, 1);
        }

        public async Task<Guid> CreateNewBookingAsync(
            Guid eventId,
            Guid userId,
            CancellationToken cancellationToken)
        {
            EventEntity? @event;
            BookingEntity? booking;

            try
            {
                await _semaphore.WaitAsync();

                @event = await _dbContext.Events.FirstAsync(e => e.Id == eventId, cancellationToken);
                var createdAt = DateTime.UtcNow;

                if (createdAt >= @event.StartAt)
                    throw new BadRequestException("Too late for booking!");

                @event.ReverseSeats();

                booking = new BookingEntity()
                {
                    CreatedAt = DateTime.UtcNow,

                    Status = BookingStatus.Pending,

                    EventId = eventId,

                    UserId = userId
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
            BookingEntity booking = await _dbContext.Bookings.FirstAsync(b => b.Id == id, cancellationToken);

            return booking;
        }

        public async Task ProcessBookingAsync(
            BookingProcessedDto bookingProcessedDto, 
            CancellationToken cancellationToken)
        {
            BookingEntity booking = await _dbContext.Bookings.FirstAsync(b => b.Id == bookingProcessedDto.Id, cancellationToken);

            if (bookingProcessedDto.Status == BookingStatus.Rejected || bookingProcessedDto.Status == BookingStatus.Cancelled)
            {
                EventEntity @event = await _dbContext.Events.FirstOrDefaultAsync(e => e.Id == booking.EventId);

                @event?.ReleaseSeats();
            }

            booking.Status = bookingProcessedDto.Status;
            booking.ProcessedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}