using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Users.Application;
using Users.Application.Contracts.Auth;
using Users.Application.Services.Auth;
using Users.Infrastructure.Postgre;
using Users.Infrastructure.Security;

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

var apiGroup = app.MapGroup("auth/api");
apiGroup.MapPost("/login", () => 
{

});

apiGroup.MapPost("/register", async (
    [FromBody] RegisterDto register, 
    IAuthService authService,
    CancellationToken cancellationToken) => 
{
    await authService.RegisterAsync(register, cancellationToken);

    return Results.NoContent();
});

app.Run();
