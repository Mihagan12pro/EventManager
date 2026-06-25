using EventManager.Application;
using EventManager.Infrastructure.PostgreSQL;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using EventManager.Tests.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

            builder.UseEnvironment("Testing");
        }
    }
}
