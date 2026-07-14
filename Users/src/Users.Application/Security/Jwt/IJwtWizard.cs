using Users.Application.Dtos.Auth;

namespace Users.Application.Security.Jwt
{
    public interface IJwtWizard
    {
        string Create(CreateTokenDto createTokenDto);
    }
}
