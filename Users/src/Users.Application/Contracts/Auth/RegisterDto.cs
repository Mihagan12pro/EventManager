using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Users.Application.Contracts.Auth
{
    public record RegisterDto(
        [Required, Length(3, 256)] string Login, 
        [Required, Length(3, 256)] string Password,
        Roles Role = Roles.User);
}
