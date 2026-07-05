using Microsoft.EntityFrameworkCore;
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

        builder.Services.AddJwtAuthentication();
        builder.Services.AddAuthorization();

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
        builder.Services.AddSecurity();
        builder.Services.AddRepositories();
        builder.Services.AddDbContext(new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build());

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();

            db.Database.Migrate();
        }

        app.UseSwaggerForDebugging();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCustomMiddleware();

        app.AddAuthEndPoints();

        app.Run();
    }
}