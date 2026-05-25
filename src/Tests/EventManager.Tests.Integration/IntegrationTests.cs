using EventManager.DataAccess.PostgreSQL.Booking;
using EventManager.DataAccess.PostgreSQL.DbContexts;
using EventManager.DataAccess.PostgreSQL.Events;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Npgsql;
using Testcontainers.PostgreSql;

namespace EventManager.Tests.Integration
{
    public abstract class IntegrationTests : IAsyncLifetime, IClassFixture<EventManagerAppFactory<Program>>
    {
        protected readonly WebApplicationFactory<Program> factory;
        protected readonly HttpClient httpClient;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            this.factory = factory;
            httpClient = factory.CreateClient();
        }

        protected readonly PostgreSqlContainer postgres = new PostgreSqlBuilder("postgres:16-alpine")
            .WithDatabase("bookstore_test")
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
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            await context.Database.MigrateAsync();
        }

        protected async Task<IServiceProvider> GetServiceProviderAsync()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<AppDbContextBase, DockerAppDbContext>(options =>
            {
                options.UseNpgsql(postgres.GetConnectionString());
            });

            services.AddValidatorsFromAssembly(typeof(Services.DependenciesInjection).Assembly);

            services.AddScoped<IEventsService, EventsService>();
            services.AddScoped<IBookingsService, BookingsService>();

            services.AddScoped<IEventsRepository, PostgreEventsRepository>();
            services.AddScoped<IBookingsRepository, PostgreBookingsRepository>();

            services.AddHostedService<BookingHandlingService>();

            services.AddLogging();

            return services.BuildServiceProvider();
        }
    }
}
