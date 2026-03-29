using EventManager.Domain.Bookings.Enums;
using EventManager.Queues.ApplicationTasks.Booking;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Bookings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventsManager.Services.Background.Bookings
{
    internal class BookingHandlingService : BackgroundService
    {
        private readonly IBookingTaskQueue _bookingTaskQueue;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_bookingTaskQueue.TryDequeue(out BookingTask bookingTask))
                    {
                        await Task.Delay(2000);

                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            IBookingService bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();


                            var booking = await bookingService.GetBookingByIdAsync(bookingTask.Id);

                            if (booking.Status == BookingStatus.Pending)
                                booking.Status = BookingStatus.Confirmed;
                        }
                    }
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation(ex, "The operation had been canceled!");
                }
                catch 
                {
                    throw;
                }

                await Task.Delay(500, stoppingToken);
            }
        }

        public BookingHandlingService(
            IBookingTaskQueue bookingTaskQueue,
            IServiceScopeFactory serviceScopeFactory,
            ILogger<BookingHandlingService> logger)
        {
            _bookingTaskQueue = bookingTaskQueue;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
    }
}
