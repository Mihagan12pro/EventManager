using EventManager.Application.Security;
using EventManager.Infrastructure.Security.Jwt;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace EventManager.Infrastructure.Security
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasherSHA256>();
            services.AddScoped<IJwtWyzard, JwtHmacSha256Wyzard>();
            services.AddScoped<IJwtClaimsExtractor, JwtClaimsExtractor>();

            return services;
        }
    }
}
