using EventManager.Application.Repositories;
using EventManager.Domain.Entities.Bookings;
using EventManager.Domain.Entities.Bookings.Enums;
using EventManager.DTOs.Bookings;
using EventManager.Shared.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventManager.Application.HostedServices.Bookings
{
    internal class BookingHandlingService : BackgroundService
    {
        private readonly SemaphoreSlim _processingSemaphore = new(1, 1);
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
                        IBookingsRepository bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingsRepository>();

                        var filters = new Filters<BookingEntity>(
                                (BookingEntity b) => (b.Status == BookingStatus.Confirmed && (b.UserId == null || b.EventId == null))
                                || b.Status == BookingStatus.Pending
                            );

                        var bookings = await bookingRepository.GetAllAsync(filters, stoppingToken);

                        var tasks = bookings.Select(pb => ProcessBookingsAsync(pb, stoppingToken)).ToArray();

                        await Task.WhenAll(tasks);
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
            BookingEntity booking,
            CancellationToken stoppingToken)
        {
            using(var scope = _serviceScopeFactory.CreateScope())
            {
                IBookingsRepository bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingsRepository>();

                BookingStatus status = booking.Status;

                try
                {
                    await _processingSemaphore.WaitAsync();

                    status = await ChangeBookingStatus(booking, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("This booking can not be processed right now because the operation had been canceled!");
                }
                finally
                {
                    _processingSemaphore.Release();

                    await bookingRepository.ProcessBookingAsync(new BookingProcessedDto(booking.Id, status), stoppingToken);
                }
            }
        }

        private async Task<BookingStatus> ChangeBookingStatus(BookingEntity booking, CancellationToken stoppingToken)
        {
            if (booking.UserId == null)
            {
                return BookingStatus.Cancelled;
            }
            else
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    IEventsRepository eventsRepository = scope.ServiceProvider.GetRequiredService<IEventsRepository>();

                    if (booking.EventId == null)
                        return BookingStatus.Rejected;

                    var @event = await eventsRepository.GetByIdAsync(booking.EventId.Value, stoppingToken);
                }

                return BookingStatus.Confirmed;
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
