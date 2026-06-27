using EventManager.Application.DataAccess.Repositories;
using EventManager.Domain.Entities.Bookings;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.Domain.Entities.Events;
using EventManager.Domain.Failures.Exceptions.WebApi.Client.BadRequest;
using EventManager.Domain.ValueObjects.Events;
using EventManager.Domain.ValueObjects.Events.DateAndTime;
using EventManager.DTOs.Bookings;
using EventManager.DTOs.Events;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Shared.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace EventManager.Tests.Integration.CRUD.Bookings
{
    public partial class BookingCrudTests : IntegrationTests
    {
        [Fact]
        public async Task Test_CreateNewBookingAsync()
        {
            await SeedResetDbAndSeedAsync();

            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();
            var bookingsRepository = provider.GetRequiredService<IBookingsRepository>();

            Guid eventId = await eventsRepository.AddNewAsync(
                new NewEventDto(
                    "Friday 13", 
                    DateTime.UtcNow.AddDays(1), 
                    DateTime.UtcNow.AddDays(2), 
                    10
                ), 
               cts.Token
            );

            var accepted = await bookingsRepository.CreateNewBookingAsync(eventId, userId, cts.Token);

            Assert.NotNull(accepted);
        }

        [Fact]
        public async Task Test_GetByIdAsync()
        {
            await SeedResetDbAndSeedAsync();

            var provider = await GetServiceProviderAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();
            var bookingsRepository = provider.GetRequiredService<IBookingsRepository>();

            Guid eventId = await eventsRepository.AddNewAsync(
                new NewEventDto(
                    "Friday 13",
                    DateTime.UtcNow.AddDays(1),
                    DateTime.UtcNow.AddDays(2),
                    10
                ),
               cts.Token
            );

            var acceptedId = await bookingsRepository.CreateNewBookingAsync(eventId, userId, cts.Token);

            var bookingModel = await bookingsRepository.GetByIdAsync(acceptedId, cts.Token);

            Assert.NotNull(bookingModel);
        }

        [Fact]
        public async Task Test_ProcessBookingAsync()
        {
            await SeedResetDbAndSeedAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            NewEventDto newEvent = new NewEventDto("Birthday", DateTime.UtcNow.AddYears(1), DateTime.UtcNow.AddYears(1).AddDays(1), 10);
            var provider = await GetServiceProviderAsync();

            var eventsRepository = provider.GetRequiredService<IEventsRepository>();
            var bookingsRepository = provider.GetRequiredService<IBookingsRepository>();

            Guid eventId = await eventsRepository.AddNewAsync(newEvent, cts.Token);
            Guid bookingId = await bookingsRepository.CreateNewBookingAsync(eventId, userId, cts.Token);

            await Task.Delay(5000);

            await bookingsRepository.ProcessBookingAsync(new BookingProcessedDto(bookingId, BookingStatus.Confirmed), cts.Token);

            var bookingModel = await bookingsRepository.GetByIdAsync(bookingId, cts.Token);

            Assert.Equal(BookingStatus.Confirmed, bookingModel.Status);
        }

        [Theory]
        [MemberData(nameof(FiltersByExpression))]
        public async Task Test_GetAllAsync_ByExpression(Expression<Func<BookingEntity, bool>> filters, int expected)
        {
            await SeedResetDbAndSeedAsync();

            await Seed();

            CancellationTokenSource cts = new CancellationTokenSource();

            var provider = await GetServiceProviderAsync();
            var bookingsRepository = provider.GetRequiredService<IBookingsRepository>();

            var bookings = await bookingsRepository.GetAllAsync(new Filters<BookingEntity>(filters), cts.Token);

            Assert.Equal(expected, bookings.Count());
        }

        [Fact]
        public async Task Test_BookingOldEvent()
        {
            await SeedResetDbAndSeedAsync();

            CancellationTokenSource cts = new CancellationTokenSource();

            var provider = await GetServiceProviderAsync();

            AppDbContext dbContext = provider.GetRequiredService<AppDbContext>();

            var @event = new EventEntity()
            {
                EventDateTime = new EventDateTime(DateTime.UtcNow, DateTime.UtcNow),

                Seats = new Seats(10),

                EventNaming = new EventNaming("Birthday"),
            };

           await dbContext.Events.AddAsync(
                    @event,
                    cts.Token
                );

            await dbContext.SaveChangesAsync();

            IBookingsRepository bookingsRepository = provider.GetRequiredService<IBookingsRepository>();

            await Assert.ThrowsAsync<BadRequestException>(async () => 
            {
                await bookingsRepository.CreateNewBookingAsync(@event.Id, userId, cts.Token);
            });
        }
    }
}
