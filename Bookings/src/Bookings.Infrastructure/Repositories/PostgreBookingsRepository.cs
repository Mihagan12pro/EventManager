using Bookings.Application.Repositories;
using Bookings.Domain;
using Bookings.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Objects.Classes.Collections;

namespace Bookings.Infrastructure.Repositories
{
    internal class PostgreBookingsRepository : IBookingRepository
    {
        private readonly BookingsDbContext _dbContext;

        private readonly ILogger<PostgreBookingsRepository> _logger;

        public async Task<Guid> CreateAsync(
            Booking booking,
            CancellationToken cancellationToken)
        {
            await _dbContext.Bookings.AddAsync(booking, cancellationToken);


            await _dbContext.SaveChangesAsync(cancellationToken);

            return booking.Id;
        }

        public async Task<IEnumerable<Booking>> GetAllWithFiltersAsync(
            Filters<Booking> filters, 
            CancellationToken cancellationToken)
        {
            IQueryable<Booking> bookings = _dbContext.Bookings;

            foreach(var filter in filters)
                bookings = bookings.Where(filter);

            return bookings;
        }

        public async Task ChangeBookingStatusAsync(
            Guid id, 
            BookingStatus status,
            DateTime processedAt,
            CancellationToken cancellationToken)
        {
            Booking booking = await _dbContext.Bookings.FirstAsync(b => b.Id == id, cancellationToken);
            var oldStatus = booking.Status;
            booking.ProcessedAt = processedAt;
            booking.Status = status;

            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "The status of the booking with id = {id} has changed from '{oldStatus}' to '{status}'",
                booking.Id,
                oldStatus, 
                status
            );
        }

        public async Task<Booking> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            Booking booking = await _dbContext.Bookings.FirstAsync(b => b.Id == id, cancellationToken);

            return booking;
        }

        public PostgreBookingsRepository(
            BookingsDbContext dbContext,
            ILogger<PostgreBookingsRepository> logger) 
        {
            _dbContext = dbContext;
            _logger = logger;
        }
    }
}
