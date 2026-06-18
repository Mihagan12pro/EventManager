using EventManager.Domain.Bookings;
using EventManager.Domain.Events;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EventManager.Infrastructure.PostgreSQL.DbContexts
{
    public class AppDbContextBase : DbContext
    {
        public AppDbContextBase(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EventEntity> Events { set; get; }

        public DbSet<BookingEntity> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
