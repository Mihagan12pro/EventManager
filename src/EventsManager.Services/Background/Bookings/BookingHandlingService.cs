using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventManager.Services.Background.Bookings
{
    internal class BookingHandlingService : BackgroundService
    {
        private readonly SemaphoreSlim _processingSemaphore = new(1, 1);
        private readonly IBookingPendingQueue _bookingTaskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IBookingsService bookingService = scope.ServiceProvider.GetRequiredService<IBookingsService>();

                        var pendingBookings = await bookingService.GetAllAsync(new BookingFiltersDto(BookingStatus.Pending, null, null));
                        var pendingTasks = pendingBookings.Select(pb => ProcessBookingsAsync(pb, stoppingToken)); 

                        await Task.WhenAll(pendingTasks);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation(ex, "The operation had been canceled!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        private async Task ProcessBookingsAsync(
            Booking booking,
            CancellationToken stoppingToken)
        {
            await Task.Delay(500);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IEventsService eventsService = scope.ServiceProvider.GetRequiredService<IEventsService>();
                IBookingsService bookingsService = scope.ServiceProvider.GetRequiredService<IBookingsService>();

                Event? eventById = null;
                
                try
                {
                    await _processingSemaphore.WaitAsync();

                    eventById = await eventsService.GetEventByIdAsync(booking.EventId);

                    booking.Confirm();
                }
                catch (NotFoundException)
                {
                    _logger.LogWarning("Event with id = {EventId} does not exists!", booking.EventId);

                    booking.Reject();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    eventById?.TryReleaseSeats();
                    booking.Reject();
                }
                finally
                {
                    _processingSemaphore.Release();
                }
            }
        }

        public BookingHandlingService(
            IBookingPendingQueue bookingTaskQueue,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<BookingHandlingService> logger)
        {
            _bookingTaskQueue = bookingTaskQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
    }
}
