using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Users.Infrastructure.Postgre
{
    internal class UsersDesignFactory 
        : IDesignTimeDbContextFactory<UsersDbContext>
    {
        public UsersDbContext CreateDbContext(string[] args)
        {
            string path = new DirectoryInfo(@"..\Users.API").FullName;

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");

            DbContextOptionsBuilder<UsersDbContext> optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new UsersDbContext(optionsBuilder.Options);
        }
    }
}
