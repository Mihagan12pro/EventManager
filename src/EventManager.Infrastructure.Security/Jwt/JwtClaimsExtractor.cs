using EventManager.Application.Security;
using Microsoft.AspNetCore.Http;

namespace EventManager.Infrastructure.Security.Jwt
{
    internal class JwtClaimsExtractor : IJwtClaimsExtractor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string Extract(string name)
            => _httpContextAccessor.HttpContext.User.FindFirst(name).Value;

        public JwtClaimsExtractor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
