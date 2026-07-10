using Bookings.Application.Repositories;
using Bookings.Domain;
using Bookings.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Shared.Objects.Interfaces;

namespace Bookings.Application.Handlers.Create
{
    internal class CreateBookingHandler 
        : ICommandHandler<Guid, CreateBookingCommand>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJwtClaimsExtractor _jwtClaimsExtractor;

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

            return await _bookingRepository.CreateAsync(booking, cancellationToken);
        }

        public CreateBookingHandler(
            IBookingRepository bookingRepository,
            IHttpContextAccessor httpContext,
            IJwtClaimsExtractor jwtClaimsExtractor)
        {
            _bookingRepository = bookingRepository;

            _jwtClaimsExtractor = jwtClaimsExtractor;
            _httpContext = httpContext;
        }
    }
}
