using Bookings.Domain;
using Bookings.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Shared.Objects.Interfaces;

namespace Bookings.Application.Handlers.Create
{
    internal class CreateBookingHandler 
        : ICommandHandler<CreateBookingCommand>
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IJwtClaimsExtractor _jwtClaimsExtractor;

        public async Task HandleAsync(
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
        }

        public CreateBookingHandler(
            IHttpContextAccessor httpContext,
            IJwtClaimsExtractor jwtClaimsExtractor)
        {
            _jwtClaimsExtractor = jwtClaimsExtractor;
            _httpContext = httpContext;
        }
    }
}
