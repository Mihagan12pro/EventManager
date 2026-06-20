using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EventManager.Infrastructure.PostgreSQL
{
    public class AppContextDesignFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            string path = new DirectoryInfo(@"..\EventManager").FullName;

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json")
                .Build();

            DbContextOptionsBuilder<AppDbContext> contextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            
            string connectionString = configuration.GetConnectionString("Default");
            contextOptionsBuilder.UseNpgsql(connectionString);

            return new AppDbContext(contextOptionsBuilder.Options);
        }
    }
}
