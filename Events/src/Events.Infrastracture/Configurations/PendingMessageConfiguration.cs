using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Messaging.Contracts.Bookings;

namespace Events.Infrastracture.Configurations
{
    internal class PendingMessageConfiguration
        : IEntityTypeConfiguration<PendingBooking>
    {
        public void Configure(EntityTypeBuilder<PendingBooking> builder)
        {
            builder.HasIndex(b => b.BookingId)
                   .IsUnique();
        }
    }
}
