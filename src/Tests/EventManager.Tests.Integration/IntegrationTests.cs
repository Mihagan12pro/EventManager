using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Application;
using EventManager.Infrastructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace EventManager.Tests.Integration
{
    public abstract class IntegrationTests : IAsyncLifetime
    {
        protected readonly PostgreSqlContainer postgres = new PostgreSqlBuilder("postgres:16-alpine")
    .WithDatabase("eventmanager_test")
    .Build();

        public async Task InitializeAsync()
        {
            await postgres.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await postgres.StopAsync();
        }

        public async Task ResetDatabaseAsync()
        {
            var provider = await GetServiceProviderAsync();

            NpgsqlConnection.ClearAllPools();
            await using var context = provider.GetRequiredService<AppDbContext>();
            var res = await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Database.MigrateAsync();
        }

        public async Task<IServiceProvider> GetServiceProviderAsync()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(postgres.GetConnectionString());
            });

            services.AddRepositories();
            services.AddBackgroundServices();
            services.AddHandlers();

            services.AddLogging();

            return services.BuildServiceProvider();
        }
    }
}
