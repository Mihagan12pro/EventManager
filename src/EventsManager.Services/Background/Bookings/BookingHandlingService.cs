using EventManager.Domain.Bookings;
using EventManager.Domain.Bookings.Enums;
using EventManager.Domain.Events;
using EventManager.DTOs.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using EventManager.Services.Exceptions.WebApi.Client.NotFound;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EventManager.Services.Background.Bookings
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

                        Expression<Func<BookingModel, bool>> filters = (BookingModel b) => b.Status == BookingStatus.Pending || (b.EventId == null && b.Status != BookingStatus.Rejected);

                        var pendingBookings = await bookingRepository.GetAllAsync(filters, stoppingToken);
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

                //await Task.Delay(500, stoppingToken);
            }
        }

        private async Task ProcessBookingsAsync(
            BookingModel booking,
            CancellationToken stoppingToken)
        {
            await Task.Delay(500);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                IEventsRepository eventsRepository = scope.ServiceProvider.GetRequiredService<IEventsRepository>();
                IBookingsRepository bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingsRepository>();

                EventModel? eventById = null;
                BookingProcessedDto bookingProcessedDto = new BookingProcessedDto(booking.Id, booking.Status);


                try
                {
                    await _processingSemaphore.WaitAsync();

                    if (booking.EventId == null)
                    {
                        bookingProcessedDto = bookingProcessedDto with
                        {
                            Status = BookingStatus.Rejected
                        };
                    }
                    else
                    {
                        eventById = await eventsRepository.GetByIdAsync(booking.EventId.Value, stoppingToken);

                        bookingProcessedDto = bookingProcessedDto with
                        {
                            Status = BookingStatus.Confirmed
                        };
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("This booking can not be processed right now because the operation had been canceled!");
                }
                finally
                {
                    await bookingRepository.ProcessBookingAsync(
                            bookingProcessedDto,
                            stoppingToken
                        );

                    _processingSemaphore.Release();

                    var a = await bookingRepository.GetByIdAsync(booking.Id, stoppingToken);
                }
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
