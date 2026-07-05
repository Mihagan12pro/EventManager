using Events.API.Api;
using Events.Application;
using Shared.AspNet.Extensions;
using Shared.Infrastructure.Security;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

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

        builder.Services.AddHandlers();
        builder.Services.AddSharedSecurity();

        builder.Services.AddJwtAuthentication();
        builder.Services.AddAuthorization();

        var app = builder.Build();

        app.UseSwaggerForDebugging();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCustomMiddleware();

        app.AddEventsEndPoints();

        app.Run();
    }
}