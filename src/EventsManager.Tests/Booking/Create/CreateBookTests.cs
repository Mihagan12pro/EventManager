using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Background_Service(NewEventDto eventDto)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddScoped<IEventsService, EventsService>();
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Adding_Two_Books_For_Event(NewEventDto eventDto)
        {
            var provider = GetProviderService();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto accepted1 = await bookingsService.CreateBookingAsync(eventId);
            BookingAcceptedDto accepted2 = await bookingsService.CreateBookingAsync(eventId);

            Assert.False(accepted1.Id == accepted2.Id);
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Booking_Pending_Status(NewEventDto eventDto)
        {
            var provider = GetProviderService();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto acceptedDto = await bookingsService.CreateBookingAsync(eventId);

            var booking = await bookingsService.GetBookingByIdAsync(acceptedDto.Id);

            Assert.Equal(BookingStatus.Pending, booking.Status);
        }

        [Fact]
        public async Task Test_Booking_Creating_Exceptions()
        {
            var provider = GetProviderService();

            IEventsService eventsService = provider.GetRequiredService<IEventsService>();
            IBookingsService bookingsService = provider.GetRequiredService<IBookingsService>();

            Guid id = Guid.Empty;

            await Assert.ThrowsAsync<NotFoundException>( () => bookingsService.CreateBookingAsync(id) );
        }
    }
}
