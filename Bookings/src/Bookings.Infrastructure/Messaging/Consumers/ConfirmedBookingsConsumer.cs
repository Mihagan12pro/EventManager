using Microsoft.Extensions.Hosting;

namespace Bookings.Infrastructure.Messaging.Consumers
{
    internal class ConfirmedBookingsConsumer : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
