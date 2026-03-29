using EventManager.Domain.Bookings;
using EventManager.Exceptions;
using EventManager.Services.Bookings;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventsManager.Services.Bookings
{
    internal class BookingService : IBookingService
    {
        private readonly List<Booking> _bookings = new List<Booking>();

        public Task<Guid> CreateBookingAsync(Guid eventId)
        {
            throw new NotImplementedException();
        }

        public async Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            Booking? booking = _bookings.FirstOrDefault(b => b.Id == bookingId);

            if (booking == null)
                throw new NotFoundException($"Booking with id = {bookingId} was not found!");

            return booking;
        }
    }
}
