using Events.API;
using Events.API.Api;
using Events.Infrastracture;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;
using Shared.AspNet.Extensions;
using System.Net.Sockets;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddTelemetry("EventsService");

        builder.Host.UseSerilog((ctx, cfg) =>
            cfg.ReadFrom.Configuration(ctx.Configuration)
            .WriteTo.Console(new CompactJsonFormatter()));


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