using Microsoft.Extensions.DependencyInjection;
using Shared.Objects.Interfaces;

namespace Shared.Infrastructure.Security
{
    public static class DependenciesInjection
    {
        /// <summary>
        /// Registers security services 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSharedSecurity(this IServiceCollection services)
        {
            services.AddScoped<IJwtClaimsExtractor, JwtClaimsExtractor>();

            return services;
        }
    }
}
