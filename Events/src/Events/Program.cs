using Events.API.Api;
using Events.Application;
using Shared.Infrastructure.Security;
using Shared.AspNet.Extensions;
using Microsoft.OpenApi;
using Events.Infrastracture;
using Microsoft.EntityFrameworkCore;

public partial class Program
{
    private static void Main(string[] args)
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

        builder.Services.AddHttpContextAccessor();


        builder.Services.AddHandlers();
        builder.Services.AddSharedSecurity();
        builder.Services.AddInfrastructure(new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build());

        builder.Services.AddJwtAuthentication();
        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.UseSwaggerForDebugging();
        app.UseHttpsRedirection();
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

        app.Run();
    }
}