using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Tests.Abstractions;
using Microsoft.AspNetCore.Hosting;
using EventManager.Handlers;
using EventManager.Infrastructure.PostgreSQL;
using EventManager.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace EventsManager.Tests.End2End
{
    public class EventManagerAppFactory<TEntryPoint>
         : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddDbContext<AppDbContextBase, DockerAppDbContext>();

                services.AddRepositories();
                services.AddBackgroundServices();
                services.AddHandlers();

                services.AddLogging();
            });

            builder.UseEnvironment("Development");
        }
    }
}
