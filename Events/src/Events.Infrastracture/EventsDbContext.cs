using Events.Infrastracture.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging.Contracts.Bookings;
using System.Reflection;

namespace Events.Infrastracture
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> contextOptions) : base(contextOptions)
        {

        }

        public DbSet<EventEntity> Events { get; set; }

        public DbSet<PendingBooking> InboxPendingMessages { get; set; }

        public DbSet<CancelledBooking> InboxCancelledMessages { get; set; }

        public DbSet<ConfirmedBooking> OutboxConfirmedBookingsMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
