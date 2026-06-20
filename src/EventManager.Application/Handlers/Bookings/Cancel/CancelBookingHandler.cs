using EventManager.Application.Repositories;

namespace EventManager.Application.Handlers.Bookings.Cancel
{
    public class CancelBookingHandler : ICommandHandler<CancelBookingCommand>
    {
        private readonly IBookingsRepository _bookingsRepository;

        public async Task HandleAsync(
            CancelBookingCommand command,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public CancelBookingHandler(IBookingsRepository bookingsRepository)
        {
            _bookingsRepository = bookingsRepository;
        }
    }
}
