using Bookings.Application.Publishers;
using Bookings.Application.Repositories;
using Bookings.Domain;
using Bookings.Domain.Enums;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Interfaces;

namespace Bookings.Application.Handlers.Cancel
{
    internal class CancelBookingHandler : ICommandHandler<CancelBookingCommand>
    {
        private readonly IPublisher _publisher;
        private readonly IBookingRepository _bookingRepository;
        private readonly IJwtClaimsExtractor _jwtClaimsExtractor;

        public async Task HandleAsync(
            CancelBookingCommand command, 
            CancellationToken cancellationToken)
        {
            Booking booking = await _bookingRepository.GetByIdAsync(
                command.Id, 
                cancellationToken
            );

            if (booking.Status != BookingStatus.Rejected || booking.Status != BookingStatus.Cancelled)
            {
                Guid userId = Guid.Parse(_jwtClaimsExtractor.Extract("sub"));

                string role = _jwtClaimsExtractor.Extract("role");

                if (role != "Admin" && userId != booking.UserId)
                    throw new ForbiddenException();

                await _bookingRepository.ChangeBookingStatusAsync(
                            command.Id,

                            BookingStatus.Cancelled,

                            DateTime.UtcNow,

                            cancellationToken
                        );

                await _publisher.ProduceAsync(
                    new CancelledBooking()
                    {
                        BookingId = booking.Id,

                        EventId = booking.EventId.Value,

                        Id = Guid.NewGuid(),

                        OccurredAt = DateTime.UtcNow,

                        UserId = userId
                    },

                    cancellationToken
                );
            }
        }

        public CancelBookingHandler(
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
