using EventManager.Domain.Bookings;
using EventManager.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace EventManager.DataAccess.PostgreSQL
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Event> Events { get; } = null;

        public DbSet<EventManager.Domain.Bookings.Booking> Bookings { get; } = null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Bookings)
                .WithOne(b => b.Event)
                .HasForeignKey(b => b.EventId);
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base (options)
        {
            
        }
    }
}
