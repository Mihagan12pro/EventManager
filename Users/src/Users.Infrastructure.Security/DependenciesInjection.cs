using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Objects.Classes;
using Users.Application.Security;
using Users.Application.Security.Jwt;
using Users.Infrastructure.Security.Jwt;

namespace Users.Infrastructure.Security
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddSecurity(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, PasswordHasherSHA256>();
            services.AddScoped<IJwtWizard, JwtHmacSha256Wyzard>();

            services.Configure<JwtToken>(configuration.GetRequiredSection("JwtOptions"));

            return services;
        }
    }
}
