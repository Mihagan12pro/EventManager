using Bookings.Application.Repositories;
using Bookings.Domain;
using Shared.Messaging.Contracts.Bookings;

namespace Bookings.Infrastructure.Repositories
{
    internal class PostgreBookingsRepository : IBookingRepository
    {
        private readonly BookingsDbContext _dbContext;

        public async Task CreateAsync(
            Booking booking,
            CancellationToken cancellationToken)
        {
            await _dbContext.Bookings.AddAsync(booking, cancellationToken);

            //await _kafkaProducer.ProduceAsync(
            //   new PendingBooking()
            //   {
            //       EventId = booking.EventId.ToString(),

            //       BookingId = booking.Id.ToString(),

            //       Id = Guid.NewGuid().ToString()
            //   },

           //    cancellationToken
           //);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public PostgreBookingsRepository(
            BookingsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
