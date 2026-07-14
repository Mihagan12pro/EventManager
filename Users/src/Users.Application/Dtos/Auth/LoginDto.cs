using System.ComponentModel.DataAnnotations;

namespace Users.Application.Dtos.Auth
{
    public record LoginDto(
        [Required] string Login, 
        [Required] string Password);
}
