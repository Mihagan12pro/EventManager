using Users.Application.Contracts.Auth;

namespace Users.Application.Security.Jwt
{
    public interface IJwtWizard
    {
        string Create(CreateTokenDto createTokenDto);
    }
}
