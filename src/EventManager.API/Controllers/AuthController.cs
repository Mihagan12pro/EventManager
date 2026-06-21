using EventManager.Application.Handlers;
using EventManager.Application.Handlers.Auth.Register;
using EventManager.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace EventManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterDto register,
            [FromServices] ICommandHandler<RegisterCommand> handler,
            CancellationToken cancellationToken)
        {
            RegisterCommand command = new RegisterCommand(register);
            await handler.HandleAsync(command, cancellationToken);

            return NoContent();
        }
    }
}
