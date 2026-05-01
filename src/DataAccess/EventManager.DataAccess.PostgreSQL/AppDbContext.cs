using EventManager.Domain.Bookings;
using EventManager.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventManager.DataAccess.PostgreSQL
{
    internal class AppDbContext : DbContext
    {
        public DbSet<EventModel> Events { set; get; }

        public DbSet<BookingModel> Bookings { get; set; }

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IConfiguration configuration) 
            : base(options)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventModel>()
                .HasMany(e => e.Bookings)
                .WithOne(b => b.Event)
                .HasForeignKey(b => b.EventId);
        }

        private readonly string _connectionString;
    }
}
