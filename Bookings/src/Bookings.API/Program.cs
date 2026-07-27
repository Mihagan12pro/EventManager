using Bookings.API.Api;
using Bookings.Application;
using Bookings.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Shared.AspNet.Extensions;
using Shared.Infrastructure.Security;

public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Host.ConfigureLogging(opt =>
        {
            opt.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error);
            opt.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error);
        });


        builder.Services.AddSwaggerGen(options =>
        {
            var binDirectory = new DirectoryInfo(AppContext.BaseDirectory);
            var files = binDirectory.GetFiles("*.xml");

            foreach (var file in files)
            {
                options.IncludeXmlComments(file.FullName);
            }

            options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("bearer", document)] = []
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddValidation();

        builder.Services.AddTelemetry("BookingsService");

        builder.Services.AddHandlers();
        builder.Services.AddInfrastructure(new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build());

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSharedSecurity();

        builder.Services.AddJwtAuthentication();
        builder.Services.AddAuthorization();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<BookingsDbContext>();

            db.Database.Migrate();
        }

        app.UseSwaggerForDebugging();
        app.UseCustomHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCustomMiddleware();

        app.AddApi();
        app.UseTelemetry();

        app.Run();
    }
}