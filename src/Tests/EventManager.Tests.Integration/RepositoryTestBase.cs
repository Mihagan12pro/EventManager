using EventManager.DataAccess.PostgreSQL.Booking;
using EventManager.DataAccess.PostgreSQL.DbContexts;
using EventManager.DataAccess.PostgreSQL.Events;
using EventManager.Services.Background.Bookings;
using EventManager.Services.Bookings;
using EventManager.Services.Events;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace EventManager.Tests.Integration
{
    public abstract class RepositoryTestBase : IAsyncLifetime
    {
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

        protected AppDbContextBase CreateContext()
        {
            DbContextOptions<AppDbContextBase> options = new DbContextOptionsBuilder<AppDbContextBase>()
                .UseNpgsql(postgres.GetConnectionString())
                .Options;

            AppDbContextBase dbContext = new AppDbContextBase(options);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        protected async Task ResetDatabaseAsync()
        {
            NpgsqlConnection.ClearAllPools();
            await using var context = CreateContext();
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

       protected IServiceProvider GetServiceProvider()
       {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<AppDbContextBase, AppDbContext>(options => {
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
