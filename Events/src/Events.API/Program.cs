using Events.API;
using Events.API.Api;
using Events.Infrastracture;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;
using Shared.AspNet.Extensions;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddTelemetry("EventsService");

        builder.Host.ConfigureLogging(options =>
        {
            options.SetMinimumLevel(LogLevel.Information);
            options.AddSerilog();

            options.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error);
            options.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error);
        })
        .UseSerilog();


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