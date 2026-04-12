using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "Create")]
        public async Task Test_CreateTwoBookingsForOneEvent(NewEventDto eventDto)
        {
            var provider = GetProviderService();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto accepted1 = await bookingsService.CreateBookingAsync(eventId);
            BookingAcceptedDto accepted2 = await bookingsService.CreateBookingAsync(eventId);

            Assert.False(accepted1.Id == accepted2.Id);
        }

        [Fact]
        [Trait("SubCategory", "Create")]
        public async Task Test_CreateBookingWithNotExistentEvent()
        {
            var provider = GetProviderService();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid id = Guid.Empty;

            await Assert.ThrowsAsync<NotFoundException>( () => bookingsService.CreateBookingAsync(id) );
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "Create")]
        public async Task Test_CreateBookingWithDeletedEvent(NewEventDto eventDto)
        {
            var provider = GetProviderService();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            var acceptedBookingDto = await bookingsService.CreateBookingAsync(eventId);
            await eventsService.DeleteAsync(eventId);

            Assert.Equal(BookingStatus.Pending, acceptedBookingDto.Status);
            await Assert.ThrowsAsync<NotFoundException>( () => bookingsService.CreateBookingAsync(eventId) );
        }
    }
}
