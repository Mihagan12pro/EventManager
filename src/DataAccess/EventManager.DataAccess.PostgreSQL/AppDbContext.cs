using EventManager.Domain.Bookings;
using EventManager.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace EventManager.DataAccess.PostgreSQL
{
    public class AppDbContext : DbContext
    {
        public DbSet<EventModel> Events { set; get; }

        public DbSet<BookingModel> Bookings { get; set; }

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IConfiguration configuration) 
            : base(options)
                => _connectionString = configuration.GetConnectionString("Default");


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_connectionString);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        private readonly string _connectionString;
    }
}
