using Microsoft.EntityFrameworkCore;

namespace Bookings.Infrastructure.SummaryTables
{
    [Keyless]
    public class PendingBookingMessage
    {
        public required Guid BookingId { get; set; }
    }
}
