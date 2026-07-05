using Microsoft.AspNetCore.Mvc;
using Shared.Infrastracture.Kafka.Producers;
using Users.API.Contracts;
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
                IKafkaProducer<JwtTokenContract> producer,
                CancellationToken cancellationToken) =>
            {
                var tokenUserId = await authService.LoginAsync(login, cancellationToken);

                JwtTokenContract contract = new JwtTokenContract(tokenUserId.Item2.ToString(), tokenUserId.Item1);

                await producer.ProduceAsync(contract, cancellationToken);

                return Results.Ok(tokenUserId.Item1);
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
