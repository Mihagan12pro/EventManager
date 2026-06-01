using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using EventManager.Tests.Unit;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Unit.Booking.Create
{
    public partial class CreateBookTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "Create")]
        public async Task Test_CreateTwoBookingsForOneEvent(NewEventDto eventDto)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto, cts.Token);

            BookingAcceptedDto accepted1 = await bookingsService.CreateBookingAsync(eventId, cts.Token);
            BookingAcceptedDto accepted2 = await bookingsService.CreateBookingAsync(eventId, cts.Token);

            Assert.False(accepted1.Id == accepted2.Id);
        }

        [Fact]
        [Trait("SubCategory", "Create")]
        public async Task Test_CreateBookingWithNotExistentEvent()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid id = Guid.Empty;

            await Assert.ThrowsAsync<NotFoundException>(() => bookingsService.CreateBookingAsync(id, cts.Token));
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "Create")]
        public async Task Test_CreateBookingWithDeletedEvent(NewEventDto eventDto)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto, cts.Token);

            var acceptedBookingDto = await bookingsService.CreateBookingAsync(eventId, cts.Token);
            await eventsService.DeleteAsync(eventId, cts.Token);

            Assert.Equal(BookingStatus.Pending, acceptedBookingDto.Status);
            await Assert.ThrowsAsync<NotFoundException>(() => bookingsService.CreateBookingAsync(eventId, cts.Token));
        }
    }
}
