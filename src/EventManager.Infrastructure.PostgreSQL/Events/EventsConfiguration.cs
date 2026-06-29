using EventManager.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManager.Infrastructure.PostgreSQL.Events
{
    public class EventsConfiguration : IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.HasMany(e => e.Bookings)
                .WithOne(b => b.Event)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(e => e.Title)
                    .HasColumnType("citext");

            builder.HasIndex(e => e.Title);
        }
    }
}
