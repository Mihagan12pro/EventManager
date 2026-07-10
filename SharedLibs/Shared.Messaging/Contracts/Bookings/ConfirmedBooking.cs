using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messaging.Contracts.Bookings
{
    public record ConfirmedBooking : BookingWithStatus
    {
        public ConfirmedBooking(string Id, string EventId, string BookingId, string OccuredAt) : base(Id, EventId, BookingId, OccuredAt)
        {
        }
    }
}
