using EventManager.Application.Security;
using Microsoft.Extensions.DependencyInjection;

namespace EventManager.Infrastructure.Security
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services.AddTransient<IPasswordHasher, PasswordHasherSHA256>();

            return services;
        }
    }
}
