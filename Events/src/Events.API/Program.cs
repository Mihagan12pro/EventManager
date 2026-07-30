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

        IConfiguration configuration = builder.Configuration;

        builder.Services.AddTelemetry(configuration, "EventsService");

        builder.Host.UseSerilog((ctx, cfg) =>
            cfg.ReadFrom.Configuration(ctx.Configuration)
            .WriteTo.Console(new CompactJsonFormatter()));


        await builder.Services.AddServices(configuration);

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