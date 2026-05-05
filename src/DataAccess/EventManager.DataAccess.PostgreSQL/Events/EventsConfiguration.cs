using EventManager.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManager.DataAccess.PostgreSQL.Events
{
    public class EventsConfiguration : IEntityTypeConfiguration<EventModel>
    {
        public void Configure(EntityTypeBuilder<EventModel> builder)
        {
            builder.HasMany(e => e.Bookings)
                .WithOne(b => b.Event)
                .HasForeignKey(b => b.EventId);

            builder.Property(e => e.Title)
                    .HasColumnType("citext");

            builder.HasIndex(e => e.Title);

            builder.HasIndex(e => e.StartAt);

            builder.HasIndex(e => e.EndAt);
        }
    }
}
