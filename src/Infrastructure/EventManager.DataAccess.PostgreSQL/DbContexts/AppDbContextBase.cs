using EventManager.Domain.Bookings;
using EventManager.Domain.Events;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EventManager.DataAccess.PostgreSQL.DbContexts
{
    public class AppDbContextBase : DbContext
    {
        public AppDbContextBase(DbContextOptions options) : base(options)
        {
        }

        public DbSet<EventModel> Events { set; get; }

        public DbSet<BookingModel> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
