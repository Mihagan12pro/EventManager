using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventManager.Tests.Booking.Background
{
    public partial class BackgroundBookingHandlingTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "BackgroundHandling")]
        public async Task Test_BackgroundService(NewEventDto eventDto)
        {
            var provider = GetProviderService();

            var hostedServices = provider.GetServices<IHostedService>();
            var cancellarationTokenSource = new CancellationTokenSource();

            var startTasks = hostedServices.Select(s => s.StartAsync(cancellarationTokenSource.Token));
            await Task.WhenAll(startTasks);

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto accepted1 = await bookingsService.CreateBookingAsync(eventId);
            BookingAcceptedDto accepted2 = await bookingsService.CreateBookingAsync(eventId);
            BookingAcceptedDto accepted3 = await bookingsService.CreateBookingAsync(eventId);
            BookingAcceptedDto accepted4 = await bookingsService.CreateBookingAsync(eventId);

            var booking1Pending = await bookingsService.GetBookingByIdAsync(accepted1.Id);
            var booking2Pending = await bookingsService.GetBookingByIdAsync(accepted2.Id);
            var booking3Pending = await bookingsService.GetBookingByIdAsync(accepted3.Id);
            var booking4Pending = await bookingsService.GetBookingByIdAsync(accepted4.Id);

            Assert.Equal(BookingStatus.Pending, booking1Pending.Status);
            Assert.Equal(BookingStatus.Pending, booking2Pending.Status);
            Assert.Equal(BookingStatus.Pending, booking3Pending.Status);
            Assert.Equal(BookingStatus.Pending, booking4Pending.Status);

            await Task.Delay(9000);

            var booking1Confirmed = await bookingsService.GetBookingByIdAsync(accepted1.Id);

            var booking2Confirmed = await bookingsService.GetBookingByIdAsync(accepted2.Id);

            var booking3Confirmed = await bookingsService.GetBookingByIdAsync(accepted3.Id);

            var booking4Confirmed = await bookingsService.GetBookingByIdAsync(accepted4.Id);

            Assert.True(booking1Confirmed.Status == BookingStatus.Rejected
                || booking1Confirmed.Status == BookingStatus.Confirmed);

            Assert.True(booking2Confirmed.Status == BookingStatus.Rejected
                || booking2Confirmed.Status == BookingStatus.Confirmed);

            Assert.True(booking3Confirmed.Status == BookingStatus.Rejected ||
                booking3Confirmed.Status == BookingStatus.Confirmed);

            Assert.True(booking4Confirmed.Status == BookingStatus.Rejected ||
                booking4Confirmed.Status == BookingStatus.Confirmed);
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "BackgroundHandling")]
        public async Task Test_BookingPendingStatus(NewEventDto eventDto)
        {
            var provider = GetProviderService();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto acceptedDto = await bookingsService.CreateBookingAsync(eventId);

            var booking = await bookingsService.GetBookingByIdAsync(acceptedDto.Id);

            Assert.Equal(BookingStatus.Pending, booking.Status);
        }
    }
}
