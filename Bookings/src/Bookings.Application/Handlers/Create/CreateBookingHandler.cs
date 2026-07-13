using Bookings.Application.Publishers;
using Bookings.Application.Repositories;
using Bookings.Domain;
using Bookings.Domain.Enums;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Interfaces;

namespace Bookings.Application.Handlers.Create
{
    internal class CreateBookingHandler 
        : ICommandHandler<Guid, CreateBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IJwtClaimsExtractor _jwtClaimsExtractor;
        private readonly IPublisher _publisher;

        public async Task<Guid> HandleAsync(
            CreateBookingCommand command,
            CancellationToken cancellationToken)
        {
            Booking booking = new Booking
            {
                CreatedAt = DateTime.UtcNow,

                Status = BookingStatus.Pending,

                EventId = command.Id,

                UserId = Guid.Parse(_jwtClaimsExtractor.Extract("sub"))
            };

            Guid id = await _bookingRepository.CreateAsync(booking, cancellationToken);

            await _publisher.ProduceAsync(
                new CancelledBooking()
                {
                    BookingId = id,
                    
                    EventId = booking.EventId.Value,
                    
                    Id = Guid.NewGuid(),
                    
                    OccurredAt = DateTime.UtcNow
                },
                
                cancellationToken
            );

            return id;
        }

        public CreateBookingHandler(
            IPublisher publisher,
            IBookingRepository bookingRepository,
            IJwtClaimsExtractor jwtClaimsExtractor)
        {
            _bookingRepository = bookingRepository;

            _jwtClaimsExtractor = jwtClaimsExtractor;

            _publisher = publisher;
        }
    }
}
