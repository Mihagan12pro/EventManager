using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Messaging.Contracts.Bookings
{
    public abstract class BookingWithStatus : IMessage
    {
        /// <summary>
        /// Message Id
        /// </summary>
        public required Guid Id { get; set; }

        public required Guid EventId { get; set; }

        public required Guid? UserId { get; set; }

        public required Guid BookingId { get; set; }

        public required DateTime OccurredAt { get; set; }
    }
}
