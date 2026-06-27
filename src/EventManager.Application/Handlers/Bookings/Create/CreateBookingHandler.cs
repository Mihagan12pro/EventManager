using EventManager.Application.DataAccess.Queries;
using EventManager.Application.DataAccess.Queries.Bodies.UsersBookings;
using EventManager.Application.DataAccess.Repositories;
using EventManager.Application.Security;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.Domain.Entities.Users;
using EventManager.DTOs.Bookings;

namespace EventManager.Application.Handlers.Bookings.Create
{
    internal class CreateBookingHandler : ICommandHandler<BookingAcceptedDto, CreateBookingCommand>
    {
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IQueryObject<int, GetUserBookingsQueryBody> _getUserQuery;
        private readonly IJwtClaimsExtractor _jwtClaimsExtractor;

        public async Task<BookingAcceptedDto> HandleAsync(
            CreateBookingCommand command, 
            CancellationToken cancellationToken)
        {
            Guid eventId = command.EventId;

            Guid? bookingId;
            BookingAcceptedDto? result = null;

            Guid userId = Guid.Parse(_jwtClaimsExtractor.Extract("sub"));

            int activeBookings = await _getUserQuery.Execute(new GetUserBookingsQueryBody(userId), cancellationToken);
            UserEntity.ValidateActiveBookings(activeBookings + 1);

            Guid id = await _bookingsRepository.CreateNewBookingAsync(eventId, userId, cancellationToken);

            result = new BookingAcceptedDto(
                id,
                eventId,
                BookingStatus.Pending);

            return result;
        }

        public CreateBookingHandler(
            IJwtClaimsExtractor jwtClaimsExtractor,
            IBookingsRepository bookingsRepository,
            IQueryObject<int, GetUserBookingsQueryBody> getUserQuery)
        {
            _getUserQuery = getUserQuery;
            _jwtClaimsExtractor = jwtClaimsExtractor;
            _bookingsRepository = bookingsRepository;
        }
    }
}
