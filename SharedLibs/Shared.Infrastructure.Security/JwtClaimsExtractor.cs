using Microsoft.AspNetCore.Http;
using Shared.Objects.Interfaces;

namespace Shared.Infrastructure.Security
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
