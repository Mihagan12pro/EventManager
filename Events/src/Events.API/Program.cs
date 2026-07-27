using Events.API.Api;
using Shared.AspNet.Extensions;
using Events.Infrastracture;
using Microsoft.EntityFrameworkCore;
using Events.API;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddTelemetry();

        builder.Host.ConfigureLogging(opt =>
        {
            opt.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error);
            opt.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error);
        });

        await builder.Services.AddServices(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build());

        var app = builder.Build();

        app.UseSwaggerForDebugging();
        app.UseCustomHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<EventsDbContext>();

            db.Database.Migrate();
        }

        app.UseCustomMiddleware();

        app.AddApi();
        app.UseTelemetry();

        app.Run();
    }
}