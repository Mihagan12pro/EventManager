using Microsoft.Extensions.DependencyInjection;

namespace Events.Application
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
