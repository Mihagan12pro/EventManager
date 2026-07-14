using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Configurations
{
    internal class PendingMessageConfiguration
        : IEntityTypeConfiguration<CancelledBooking>
    {
        public void Configure(EntityTypeBuilder<CancelledBooking> builder)
        {
            builder.HasIndex(b => b.BookingId)
                   .IsUnique();
        }
    }
}
