using EventManager.Domain.Entities.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManager.DataAccess.PostgreSQL.Booking
{
    public class BookingsConfiguration : IEntityTypeConfiguration<BookingEntity>
    {
        public void Configure(EntityTypeBuilder<BookingEntity> builder)
        {
            builder.Property(b => b.EventId)
                .IsRequired(false);

            builder.Property(b => b.Status)
                .HasConversion<string>();
        }
    }
}
