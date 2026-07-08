using Bookings.Application.Repositories;
using Bookings.Domain;
using Shared.Infrastracture.Kafka.Producers;
using Shared.Messaging.Contracts;

namespace Bookings.Infrastructure.Repositories
{
    internal class PostgreBookingsRepository : IBookingRepository
    {
        private readonly BookingsDbContext _dbContext;
        private readonly IKafkaProducer<PendingBooking> _kafkaProducer;

        public async Task CreateAsync(
            Booking booking,
            CancellationToken cancellationToken)
        {
            await _dbContext.Bookings.AddAsync(booking, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await _kafkaProducer.ProduceAsync(
                new PendingBooking()
                {
                    EventId = booking.EventId.ToString(),

                    BookingId = booking.Id.ToString(),

                    Id = Guid.NewGuid().ToString()
                },

                cancellationToken
            );
        }

        public PostgreBookingsRepository(
            BookingsDbContext dbContext,
            IKafkaProducer<PendingBooking> kafkaProducer)
        {
            _dbContext = dbContext;

            _kafkaProducer = kafkaProducer;
        }
    }
}
