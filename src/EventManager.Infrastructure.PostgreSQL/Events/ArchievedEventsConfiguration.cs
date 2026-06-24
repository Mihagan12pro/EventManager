using EventManager.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManager.Infrastructure.PostgreSQL.Events
{
    public class ArchievedEventsConfiguration : IEntityTypeConfiguration<ArchivedEventEntity>
    {
        public void Configure(EntityTypeBuilder<ArchivedEventEntity> builder)
        {
            builder.HasOne(ae => ae.Event)
                   .WithOne(e => e.Archived)
                   .HasForeignKey<ArchivedEventEntity>(ae => ae.EventId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
