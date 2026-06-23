using EventManager.Application.DataAccess.Queries;
using EventManager.Application.DataAccess.Queries.Bodies.UsersBookings;
using EventManager.Application.DataAccess.Repositories;
using EventManager.Application.Security;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.Domain.Entities.Users.Enums;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.Conflict;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.Forbidden;
using EventManager.DTOs.Bookings;

namespace EventManager.Application.Handlers.Bookings.Cancel
{
    public class CancelBookingHandler : ICommandHandler<CancelBookingCommand>
    {
        private readonly IQueryObject<CompareUserBookingQueryBody> _queryObjectComparer;
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IJwtClaimsExtractor _jwtClaimsExtractor;

        public async Task HandleAsync(
            CancelBookingCommand command,
            CancellationToken cancellationToken)
        {
            var role = _jwtClaimsExtractor.Extract("role");
            if (role == nameof(Roles.User))
            {
                Guid userId = Guid.Parse(_jwtClaimsExtractor.Extract("sub"));
                //Guid.TryParse(_jwtClaimsExtractor.Extract("sub"), out Guid userId);

                CompareUserBookingQueryBody compareUserBookingQuery = new(command.BookingId, userId);

                try
                {
                    await _queryObjectComparer.Execute(compareUserBookingQuery, cancellationToken);
                }
                catch (ConflictException)
                {
                    throw new ForbiddenException("This user has no right to cancel this book!");
                }
            }

            await _bookingsRepository.ProcessBookingAsync(new BookingProcessedDto(command.BookingId, BookingStatus.Cancelled), cancellationToken);
        }

        public CancelBookingHandler(
            IBookingsRepository bookingsRepository,
            IJwtClaimsExtractor jwtClaimsExtractor,
            IQueryObject<CompareUserBookingQueryBody> queryObjectComparer)
        {
            _bookingsRepository = bookingsRepository;

            _jwtClaimsExtractor = jwtClaimsExtractor;

            _queryObjectComparer = queryObjectComparer;
        }
    }
}
