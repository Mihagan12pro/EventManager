using EventManager.Application.Handlers;
using EventManager.Application.Handlers.Bookings.Create;
using EventManager.Application.Handlers.Events.AddEvent;
using EventManager.Application.Handlers.Events.DeleteEvent;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.NotFound;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
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

            var addingEventHandler = provider.GetRequiredService<ICommandHandler<Guid, AddEventCommand>>();
            var createBookingHandler = provider.GetRequiredService<ICommandHandler<BookingAcceptedDto, CreateBookingCommand>>();

            Guid eventId = await addingEventHandler.HandleAsync(new AddEventCommand(eventDto), cts.Token);

            BookingAcceptedDto accepted1 = await createBookingHandler.HandleAsync(new CreateBookingCommand(eventId), cts.Token);
            BookingAcceptedDto accepted2 = await createBookingHandler.HandleAsync(new CreateBookingCommand(eventId), cts.Token);

            Assert.False(accepted1.Id == accepted2.Id);
        }

        [Fact]
        [Trait("SubCategory", "Create")]
        public async Task Test_CreateBookingWithNotExistentEvent()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            var creatingBookingsHandler = provider.GetRequiredService<ICommandHandler<BookingAcceptedDto, CreateBookingCommand>>();

            Guid eventId = Guid.Empty;

            await Assert.ThrowsAsync<NotFoundException>(() => creatingBookingsHandler.HandleAsync(new CreateBookingCommand(eventId), cts.Token));
        }

        [Theory]
        [MemberData(nameof(AddEvents))]
        [Trait("SubCategory", "Create")]
        public async Task Test_CreateBookingWithDeletedEvent(NewEventDto eventDto)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            var provider = TestingServicesProvider.GetServicesProvider();

            var addingEventsHandler = provider.GetRequiredService<ICommandHandler<Guid, AddEventCommand>>();
            var deletingEventsHandle = provider.GetRequiredService<ICommandHandler<string, DeleteEventCommand>>();
            var creatingBookingsHandler = provider.GetRequiredService < ICommandHandler<BookingAcceptedDto, CreateBookingCommand>>();
            
            Guid eventId = await addingEventsHandler.HandleAsync(new AddEventCommand(eventDto), cts.Token);

            var acceptedBookingDto = await creatingBookingsHandler.HandleAsync(new CreateBookingCommand(eventId), cts.Token);
            await deletingEventsHandle.HandleAsync(new DeleteEventCommand(eventId), cts.Token);

            Assert.Equal(BookingStatus.Pending, acceptedBookingDto.Status);
            await Assert.ThrowsAsync<NotFoundException>(() => creatingBookingsHandler.HandleAsync(new CreateBookingCommand(eventId), cts.Token));
        }
    }
}
