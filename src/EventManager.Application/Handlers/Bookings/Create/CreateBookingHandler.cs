using EventManager.Application.Repositories;
using EventManager.Application.Security;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.DTOs.Bookings;

namespace EventManager.Application.Handlers.Bookings.Create
{
    internal class CreateBookingHandler : ICommandHandler<BookingAcceptedDto, CreateBookingCommand>
    {
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IJwtClaimsExtractor _jwtClaimsExtractor;

        public async Task<BookingAcceptedDto> HandleAsync(
            CreateBookingCommand command, 
            CancellationToken cancellationToken)
        {
            Guid eventId = command.EventId;

            Guid? bookingId;
            BookingAcceptedDto? result = null;

            Guid userId = Guid.Parse(_jwtClaimsExtractor.Extract("sub"));

            result = new BookingAcceptedDto(
                userId,
                eventId,
                BookingStatus.Pending);

            await _bookingsRepository.CreateNewBookingAsync(eventId, userId, cancellationToken);

            return result;
        }

        public CreateBookingHandler(
            IJwtClaimsExtractor jwtClaimsExtractor,
            IBookingsRepository bookingsRepository)
        {
            _jwtClaimsExtractor = jwtClaimsExtractor;
            _bookingsRepository = bookingsRepository;
        }
    }
}
