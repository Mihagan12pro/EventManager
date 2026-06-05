using EventManager.Infrastructure.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EventManager.Infrastructure.PostgreSQL.DbContexts.Design
{
    internal class DesignTimerAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            string path = new DirectoryInfo(@"..\..\EventManager").FullName;

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json")
                .Build();

            DbContextOptionsBuilder<AppDbContext> contextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            return new AppDbContext(contextOptionsBuilder.Options, configuration);
        }
    }
}
