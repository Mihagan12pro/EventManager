using EventManager.Domain.Entities.Bookings;
using EventManager.Domain.Entities.Events;
using EventManager.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace EventManager.Infrastructure.PostgreSQL.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public DbSet<EventEntity> Events { set; get; }

        public DbSet<BookingEntity> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
        {
            
        }
    }
}
