using Bookings.Infrastructure.SummaryTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookings.Infrastructure.Configurations
{
    internal class PendingBookingMessageConfiguration
         : IEntityTypeConfiguration<PendingBookingMessage>
    {
        public void Configure(EntityTypeBuilder<PendingBookingMessage> builder)
        {
            builder.HasIndex(pbm => pbm.BookingId)
                   .IsUnique();
        }
    }
}
