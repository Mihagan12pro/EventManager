using Microsoft.AspNetCore.Mvc;
using Users.Application.Dtos.Auth;
using Users.Application.Services.Auth;

namespace Users.API.Api
{
    public static class AuthApi
    {
        public static WebApplication AddAuthEndPoints(this WebApplication app)
        {
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

            return app;
        }
    }
}
