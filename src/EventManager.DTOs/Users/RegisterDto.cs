using EventManager.Domain.Entities.Users.Enums;
using System.ComponentModel.DataAnnotations;

namespace EventManager.DTOs.Users
{
    public record RegisterDto(
        [MinLength(3), MaxLength(256)] string Login, 
        [MinLength(3)] string Password,
        Roles Role = Roles.User);
}
