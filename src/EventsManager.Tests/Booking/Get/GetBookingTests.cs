using EventManager.Services.Bookings;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Booking.Get
{
    public partial class GetBookingTests
    {
        [Fact]
        [Trait("SubCategory", "Get")]
        public async Task Test_GetNotExistentBooking()
        {
            var provider = GetProviderService();

            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();
            await Assert.ThrowsAsync<NotFoundException>(() => bookingsService.GetBookingByIdAsync(Guid.Empty));
        }
    }
}
