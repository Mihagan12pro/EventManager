using EventManager.Infrastructure.PostgreSQL;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;
using EventManager.Application;

namespace EventManager.Tests.Abstractions
{
    public abstract class RealPostgreTests : IAsyncLifetime
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

        protected async Task ResetDatabaseAsync()
        {
            var provider = await GetServiceProviderAsync();

            NpgsqlConnection.ClearAllPools();
            await using var context = provider.GetRequiredService<AppDbContextBase>();
            var res = await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Database.MigrateAsync();
        }

        protected virtual async Task<IServiceProvider> GetServiceProviderAsync()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<AppDbContextBase, DockerAppDbContext>(options =>
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

