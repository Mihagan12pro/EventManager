using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;
using Shared.AspNet.Extensions;
using Users.API.Api;
using Users.Application;
using Users.Infrastructure.Postgre;
using Users.Infrastructure.Security;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        builder.Services.AddJwtAuthentication(configuration);
        builder.Services.AddAuthorization();

        builder.Services.AddTelemetry(configuration, "UsersService");

        builder.Host.UseSerilog((ctx, cfg) =>
            cfg.ReadFrom.Configuration(ctx.Configuration)
            .WriteTo.Console(new CompactJsonFormatter()));



        builder.Services.AddSwaggerGen(options =>
        {
            var binDirectory = new DirectoryInfo(AppContext.BaseDirectory);
            var files = binDirectory.GetFiles("*.xml");

            foreach (var file in files)
            {
                options.IncludeXmlComments(file.FullName);
            }
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddValidation();

        builder.Services.AddAuthServises();
        builder.Services.AddSecurity(configuration);
        builder.Services.AddRepositories();
        builder.Services.AddDbContext(configuration);

        var app = builder.Build();

        app.UseSwaggerForDebugging();
        app.UseCustomHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCustomMiddleware();

        app.AddApi();

        app.UseTelemetry();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

            db.Database.Migrate();
        }

        app.Run();
    }
}