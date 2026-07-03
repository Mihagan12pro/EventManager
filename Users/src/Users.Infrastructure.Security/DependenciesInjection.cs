using Microsoft.Extensions.DependencyInjection;
using Users.Application.Security;

namespace Users.Infrastructure.Security
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasherSHA256>();

            return services;
        }
    }
}
