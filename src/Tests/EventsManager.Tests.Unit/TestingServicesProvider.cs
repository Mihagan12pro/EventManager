using EventManager.Application;
using EventManager.Infrastructure.PostgreSQL;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Infrastructure.Security;
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

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            services.AddHandlers();
            services.AddBackgroundServices();
            services.AddRepositories();
            services.AddSecurity();
            services.AddQueries();

            services.AddLogging();

            services.AddHttpContextAccessor();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
