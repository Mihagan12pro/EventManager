using EventManager.Services.Bookings;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using EventManager.Services.Tests;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Booking.Get
{
    public partial class GetBookingTests
    {
        [Fact]
        [Trait("SubCategory", "Get")]
        public async Task Test_GetNotExistentBooking()
        {

            CancellationTokenSource cts = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();
            await Assert.ThrowsAsync<NotFoundException>(() => bookingsService.GetBookingByIdAsync(Guid.Empty, cts.Token));
        }
    }
}
