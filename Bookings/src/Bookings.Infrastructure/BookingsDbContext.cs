using Bookings.Domain;
using Bookings.Infrastructure.SummaryTables;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging;
using System.Reflection;

namespace Bookings.Infrastructure
{
    public class BookingsDbContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Message> InboxMessages { get; set; }


        public BookingsDbContext(DbContextOptions<BookingsDbContext> contextOptions) : base(contextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
