using EventManager;
using EventManager.Infrastructure.PostgreSQL.DbContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddServices();

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
        });

        builder.Services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => 
        {
            var jwtSection = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                                                       .Build()
                                                       .GetRequiredSection("JwtOptions");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = true,

                ValidateIssuer = true,
                ValidIssuer = jwtSection.GetRequiredSection("Issuer").Value,

                ValidateAudience = true,
                ValidAudience = jwtSection.GetRequiredSection("Audience").Value,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSection.GetRequiredSection("SecretKey").Value))
            };
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContextBase>();

            db.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCustomMiddleware();

        app.MapControllers();

        app.Run();
    }
}