using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace EventManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Adding_Two_Books_For_Event(NewEventDto eventDto)
        {
            IEventsService eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);
            IBookingsService bookingsService = (IBookingsService)Activator.CreateInstance(_bookingsServiceType, eventsService);

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto accepted1 = await bookingsService.CreateBookingAsync(eventId);
            BookingAcceptedDto accepted2 = await bookingsService.CreateBookingAsync(eventId);

            Assert.False(accepted1.Id == accepted2.Id);
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Booking_Pending_Status(NewEventDto eventDto)
        {
            IEventsService eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);
            IBookingsService bookingsService = (IBookingsService)Activator.CreateInstance(_bookingsServiceType, eventsService);

            Guid eventId = await eventsService.AddNewAsync(eventDto);

            BookingAcceptedDto acceptedDto = await bookingsService.CreateBookingAsync(eventId);

            var booking = await bookingsService.GetBookingByIdAsync(acceptedDto.Id);

            Assert.Equal(BookingStatus.Pending, booking.Status);
        }

        [Fact]
        public async Task Test_Booking_Creating_Exceptions()
        {
            IEventsService eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);
            IBookingsService bookingsService = (IBookingsService)Activator.CreateInstance(_bookingsServiceType, eventsService);

            Guid id = Guid.Empty;

            await Assert.ThrowsAsync<NotFoundException>( () => bookingsService.CreateBookingAsync(id) );
        }
    }
}
