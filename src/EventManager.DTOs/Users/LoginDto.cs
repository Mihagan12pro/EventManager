using System.ComponentModel.DataAnnotations;

namespace EventManager.DTOs.Users
{
    public record LoginDto(
        [MinLength(3), MaxLength(256), Required(AllowEmptyStrings = false)] string Login,
        [MinLength(3)] string Password);
}
