using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventsManager.Services.Background.Bookings
{
    internal class BookingHandlingService : BackgroundService
    {
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
                        
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Thrown exception: {ex}", ex);
                }

                await Task.Delay(500, stoppingToken);
            }
        }

        public BookingHandlingService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<BookingHandlingService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
    }
}
