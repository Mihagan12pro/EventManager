using Bookings.Application.Repositories;
using Bookings.Domain;
using Bookings.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Collections;

namespace Bookings.Infrastructure.Repositories
{
    internal class PostgreBookingsRepository : IBookingRepository
    {
        private readonly BookingsDbContext _dbContext;

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

        public async Task ChangeBookingStatus(
            Guid id, 
            BookingStatus status,
            CancellationToken cancellationToken)
        {
            Booking booking = await _dbContext.Bookings.FirstAsync(b => b.Id == id, cancellationToken);
            booking.Status = status;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public PostgreBookingsRepository(
            BookingsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
