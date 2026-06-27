using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Application;
using EventManager.Infrastructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;
using EventManager.Domain.Entities.Users;
using EventManager.Domain.ValueObjects.Users;
using EventManager.Domain.Entities.Users.Enums;

namespace EventManager.Tests.Integration
{
    public abstract class IntegrationTests : IAsyncLifetime
    {
        protected Guid userId;

        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
             .WithDatabase("eventmanager_test")
             .WithUsername("postgres_tests")
             .WithPassword("postgres_tests")
             .Build();

        protected async Task SeedResetDbAndSeedAsync()
        {
            await ResetDatabaseAsync();

            var provider = await GetServiceProviderAsync();

            var dbContext = provider.GetRequiredService<AppDbContext>();

            UserEntity user = new UserEntity() 
            {
                HashedPassword = "hashed_password",
                UserName = new UserName("Admin"),
                Role = Roles.Admin 
            };

            await dbContext.Users.AddAsync(user);

            await dbContext.SaveChangesAsync();

            userId = user.Id;
        }

        public async Task InitializeAsync()
        {
            await _postgres.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _postgres.StopAsync();
        }

        public async Task ResetDatabaseAsync()
        {
            NpgsqlConnection.ClearAllPools();

            var provider = await GetServiceProviderAsync();
            
            using var scope = provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.Database.EnsureDeletedAsync();
            await db.Database.MigrateAsync();

            await db.Database.CloseConnectionAsync();
            NpgsqlConnection.ClearAllPools();
        }

        public async Task<IServiceProvider> GetServiceProviderAsync()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(_postgres.GetConnectionString());
            });

            services.AddRepositories();
            services.AddBackgroundServices();
            services.AddHandlers();

            services.AddLogging();

            return services.BuildServiceProvider();
        }
    }
}
