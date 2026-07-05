using System.ComponentModel.DataAnnotations;

namespace Users.Application.Contracts.Auth
{
    public record LoginDto(
        [Required] string Login, 
        [Required] string Password);
}
