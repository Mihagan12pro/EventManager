using EventManager.Domain.Bookings.Enums;
using EventManager.Queues.ApplicationTasks.Booking;
using EventManager.Queues.Queues.Booking;
using EventManager.Services.Bookings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EventManager.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
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
                    if (_bookingTaskQueue.TryDequeue(out BookingTask bookingTask))
                    {
                        await Task.Delay(2000);

                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            IBookingsService bookingService = scope.ServiceProvider.GetRequiredService<IBookingsService>();


                            var booking = await bookingService.GetBookingByIdAsync(bookingTask.Id);

                            if (booking.Status == BookingStatus.Pending)
                            {
                                booking.Status = BookingStatus.Confirmed;
                                booking.ProcessedAt = DateTime.Now;
                            }
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

                await Task.Delay(3000, stoppingToken);
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
