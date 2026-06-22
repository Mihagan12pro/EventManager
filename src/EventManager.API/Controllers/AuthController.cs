using EventManager.Application.Handlers;
using EventManager.Application.Handlers.Auth.Login;
using EventManager.Application.Handlers.Auth.Register;
using EventManager.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterDto register,
            [FromServices] ICommandHandler<RegisterCommand> handler,
            CancellationToken cancellationToken)
        {
            RegisterCommand command = new RegisterCommand(register);
            await handler.HandleAsync(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(
            [FromBody] LoginDto login,
            [FromServices] ICommandHandler<string, LoginCommand> handler,
            CancellationToken cancellationToken) 
        {
            LoginCommand command = new LoginCommand(login);

            string token = await handler.HandleAsync(command, cancellationToken);

            return Ok(new { Token = token });
        }
    }
}
