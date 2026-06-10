using EventManager.Application;
using EventManager.Infrastructure.PostgreSQL;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Tests.Unit
{
    internal static class TestingServicesProvider
    {
        public static IServiceProvider GetServicesProvider()
        {
            IServiceCollection services = new ServiceCollection();

            var dbName = Guid.NewGuid().ToString();

            services.AddDbContext<AppDbContextBase, InMemoryAppDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            services.AddHandlers();
            services.AddBackgroundServices();
            services.AddRepositories();

            services.AddLogging();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
