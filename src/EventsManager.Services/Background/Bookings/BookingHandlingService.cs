using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Bookings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventManager.Services.Background.Bookings
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
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IBookingsService bookingService = scope.ServiceProvider.GetRequiredService<IBookingsService>();

                        var bookings = await bookingService.GetAllAsync(new BookingFiltersDto(BookingStatus.Pending, null, null));
                        
                        foreach(var booking in bookings)
                        {
                            await RequestToRemoteSystemAsync(booking, stoppingToken);

                            booking.ProcessedAt = DateTime.Now;
                        }
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

        private async Task RequestToRemoteSystemAsync(
            Booking booking,
            CancellationToken cancellationToken)
        {
            await Task.Delay(2000);

            Random random = new Random();
            int next = random.Next(2);

            if (next == 1)
                booking.Status = BookingStatus.Rejected;
            else
                booking.Status = BookingStatus.Confirmed;
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
