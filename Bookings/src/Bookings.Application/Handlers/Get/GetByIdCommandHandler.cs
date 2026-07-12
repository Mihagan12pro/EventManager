using Bookings.Application.Dtos;
using Bookings.Application.Repositories;
using Bookings.Domain;
using Microsoft.AspNetCore.Http;
using Shared.Failures.Exceptions.WebApi.ClientErrors;
using Shared.Objects.Interfaces;

namespace Bookings.Application.Handlers.Get
{
    internal class GetByIdCommandHandler : ICommandHandler<GetBookingDto, GetByIdCommand>
    {
        private readonly IHttpContextAccessor _httpContext;

        private readonly IBookingRepository _bookingRepository;

        private readonly IJwtClaimsExtractor _claimsExtractor;

        public async Task<GetBookingDto> HandleAsync(
            GetByIdCommand command, 
            CancellationToken cancellationToken)
        {
            Booking booking = await _bookingRepository.GetByIdAsync(command.BookingId, cancellationToken);

            if (booking.UserId != Guid.Parse(_claimsExtractor.Extract("sub")))
                throw new ForbiddenException();


            return new GetBookingDto(
                booking.Id, 
                
                booking.EventId.Value,
                
                booking.ProcessedAt, 
            
                booking.Status
            );
        }

        public GetByIdCommandHandler(
            IBookingRepository bookingRepository,
            IHttpContextAccessor httpContext,
            IJwtClaimsExtractor claimsExtractor)
        {
            _httpContext = httpContext;

            _bookingRepository = bookingRepository;

            _claimsExtractor = claimsExtractor;
        }
    }
}
