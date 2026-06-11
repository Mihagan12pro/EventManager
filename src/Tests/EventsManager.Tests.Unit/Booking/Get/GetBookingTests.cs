using EventManager.Application.Handlers;
using EventManager.Application.Handlers.Bookings.GetByIdBooking;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.NotFound;
using EventManager.DTOs.Bookings;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Unit.Booking.Get
{
    public partial class GetBookingTests
    {
        [Fact]
        [Trait("SubCategory", "Get")]
        public async Task Test_GetNotExistentBooking()
        {

            CancellationTokenSource cts = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            var gettingHandler = provider.GetRequiredService<ICommandHandler<GetBookingDto, GetByIdBookingCommand>>();
            await Assert.ThrowsAsync<NotFoundException>(() => gettingHandler.HandleAsync(new GetByIdBookingCommand(Guid.Empty), cts.Token));
        }
    }
}
