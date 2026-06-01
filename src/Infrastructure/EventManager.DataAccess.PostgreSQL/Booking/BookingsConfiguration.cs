using EventManager.Domain.Bookings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManager.DataAccess.PostgreSQL.Booking
{
    public class BookingsConfiguration : IEntityTypeConfiguration<BookingModel>
    {
        public void Configure(EntityTypeBuilder<BookingModel> builder)
        {
            builder.Property(b => b.EventId)
                .IsRequired(false);
        }
    }
}
