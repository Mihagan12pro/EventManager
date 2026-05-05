using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventManager.DataAccess.PostgreSQL
{
    internal class AppDbContext : AppDbContextBase
    {
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration) 
        {
            _connectionString = configuration.GetConnectionString("Default");
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(_connectionString);

        private readonly string _connectionString;
    }
}
