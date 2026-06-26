using EventManager.Application;
using EventManager.Application.DataAccess.Queries;
using EventManager.Infrastructure.PostgreSQL;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Tests.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Reflection;
using Testcontainers.PostgreSql;

namespace EventsManager.Tests.End2End
{
    public class EventManagerAppFactory<TEntryPoint>
         : WebApplicationFactory<TEntryPoint>, IAsyncLifetime, IRealPostgreTests where TEntryPoint : class
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
             .WithDatabase("eventmanager_test")
             .Build();

        public async Task InitializeAsync()
        {
            await _postgres.StartAsync();
        }

        public new async Task DisposeAsync()
        {
            Dispose();
            await _postgres.DisposeAsync();
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<AppDbContextBase, AppDbContext>(options =>
                {
                    options.UseNpgsql(
                     _postgres.GetConnectionString(),
                     npgsqlOptions =>
                     {
                         npgsqlOptions.MigrationsAssembly(typeof(AppDbContextBase).Assembly.FullName);
                     });
                });

                services.AddRepositories();

                Assembly assembly = typeof(AppDbContextBase).Assembly;

                services.Scan(scan => scan.FromAssemblies(assembly)
                   .AddClasses(classes => classes
                       .AssignableToAny(
                           typeof(IQueryObject<,>),
                           typeof(IQueryObject<>)
                       ),
                       publicOnly: false
                   )
                   .AsSelfWithInterfaces()
               .WithScopedLifetime());
            });


            builder.UseEnvironment("Testing");
        }

        public async Task ResetDatabaseAsync()
        {
            NpgsqlConnection.ClearAllPools();

            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContextBase>();

            await db.Database.EnsureDeletedAsync();
            await db.Database.MigrateAsync();

            await db.Database.CloseConnectionAsync();
            NpgsqlConnection.ClearAllPools();
        }
    }
}
