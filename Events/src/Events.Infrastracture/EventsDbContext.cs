using Events.Domain;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging;
using System.Reflection;

namespace Events.Infrastracture
{
    public class EventsDbContext : DbContext
    {
        public EventsDbContext(DbContextOptions<EventsDbContext> contextOptions) : base(contextOptions)
        {

        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Message> Outbox { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
