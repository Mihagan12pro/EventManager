using System;
using System.Collections.Generic;
using System.Text;
using Users.Domain.Enums;

namespace Users.Application.Contracts.Auth
{
    public record CreateTokenDto(
        string Login, 
        Guid UserId, 
        Roles Role);
}
