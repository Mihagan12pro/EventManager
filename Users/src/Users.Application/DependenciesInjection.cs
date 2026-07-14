using Microsoft.Extensions.DependencyInjection;
using Users.Application.Services.Auth;

namespace Users.Application
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddAuthServises(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
