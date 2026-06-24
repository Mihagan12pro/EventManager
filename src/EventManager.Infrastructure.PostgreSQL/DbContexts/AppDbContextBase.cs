using EventManager.Domain.Entities.Bookings;
using EventManager.Domain.Entities.Events;
using EventManager.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EventManager.Infrastructure.PostgreSQL.DbContexts
{
    public class AppDbContextBase : DbContext
    {
        public AppDbContextBase(DbContextOptions options) : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<EventEntity> Events { set; get; }

        public DbSet<ArchivedEventEntity> ArchivedEvents { get; set; }

        public DbSet<BookingEntity> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
