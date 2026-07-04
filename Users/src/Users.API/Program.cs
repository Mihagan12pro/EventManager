using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.AspNet.Extensions;
using Shared.Objects;
using System.Text;
using Users.Application;
using Users.Application.Contracts.Auth;
using Users.Application.Services.Auth;
using Users.Infrastructure.Postgre;
using Users.Infrastructure.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var authOptions = new AuthOptions();


    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,

        ValidateLifetime = true,

        ValidateIssuer = true,
        ValidIssuer = authOptions.Issuer,

        ValidateAudience = true,
        ValidAudiences = authOptions.Audiences,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(authOptions.IssuerSigningKey),

        RoleClaimType = "role"
    };
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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseCustomDbExceptionsMiddleware();
app.UseCustomWebApiExceptionsMiddleware();
//app.UseAuthorization();

var apiGroup = app.MapGroup("auth/api");
apiGroup.MapPost("/login", async (
    [FromBody] LoginDto login,
    IAuthService authService,
    CancellationToken cancellationToken) => 
{
    string token = await authService.LoginAsync(login, cancellationToken);

    return Results.Ok(token);
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
