using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions;
using Moq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Background.Bookings;

namespace EventManager.Tests.Booking.Create
{
    public partial class CreateBookTests
    {
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

        [Theory]
        [MemberData(nameof(AddEvents))]
        public async Task Test_Background_Service(NewEventDto eventDto)
        {
            IEventsService eventsService = (IEventsService)Activator.CreateInstance(_eventsServiceType);
            IBookingsService bookingsService = (IBookingsService)Activator.CreateInstance(_bookingsServiceType, eventsService);

            var queue = new Mock<IBookingTaskQueue>();
            var factory = new Mock<IServiceScopeFactory>();
            var logger = new Mock<ILogger<BookingHandlingService>>();

            BookingHandlingService bookingHandlingService = new BookingHandlingService(queue.Object, factory.Object, logger.Object);
        }
    }
}
