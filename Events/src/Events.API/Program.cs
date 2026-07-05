using Shared.AspNet.Extensions;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.UseSwaggerForDebugging();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCustomMiddleware();

        var apiGroup = app.MapGroup("events/api");

        apiGroup.MapPost("", async(CancellationToken token) => 
        {

        });
        //apiGroup.MapPost("/login", async (
        //    [FromBody] LoginDto login,
        //    IAuthService authService,
        //    CancellationToken cancellationToken) =>
        //{
        //    string token = await authService.LoginAsync(login, cancellationToken);

        //    return Results.Ok(token);
        //});

        //apiGroup.MapPost("/register", async (
        //    [FromBody] RegisterDto register,
        //    IAuthService authService,
        //    CancellationToken cancellationToken) =>
        //{
        //    await authService.RegisterAsync(register, cancellationToken);

        //    return Results.NoContent();
        //});

        app.Run();
    }
}