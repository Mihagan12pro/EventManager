using Bookings.Application.Repositories;
using Bookings.Domain;
using Bookings.Domain.Enums;
using Bookings.Infrastructure.Messaging.Publishers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Messaging.Contracts.Bookings;
using Shared.Objects.Classes.Collections;

namespace Bookings.Infrastructure.Hosted
{
    internal class PendingBookingsHandler : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IPublisher _publisher;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        IBookingRepository bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();

                        var pendingBookings = await bookingRepository.GetAllWithFiltersAsync(
                                new Filters<Booking>() 
                                {
                                    (Booking b) => b.Status == BookingStatus.Pending
                                },

                                stoppingToken
                            );

                        var tasks = pendingBookings.Select(pb => SendMessageAsync(pb, stoppingToken)).ToArray();

                        await Task.WhenAll(tasks);
                    }
                }
                catch (OperationCanceledException ex)
                {
                    
                }
            }
        }

        private async Task SendMessageAsync(
            Booking pendingBooking, 
            CancellationToken stoppingToken)
        {
            PendingBooking pendingMessage = new PendingBooking(
                Guid.NewGuid().ToString(),
                pendingBooking.EventId.ToString(),
                pendingBooking.Id.ToString(), 
                DateTime.UtcNow.ToString());

            await _publisher.ProduceAsync(pendingMessage, stoppingToken);
        }


        public PendingBookingsHandler(
            IServiceScopeFactory serviceScopeFactory,
            IPublisher publisher)
        {
            _publisher = publisher;

            _serviceScopeFactory = serviceScopeFactory;
        }
    }
}
