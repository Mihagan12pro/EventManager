using Events.Infrastracture;
using Events.Application;
using Shared.Infrastructure.Security;
using Shared.AspNet.Extensions;

namespace Events.API
{
    public static class DependenciesInjection
    {
        public static async Task<IServiceCollection> AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHandlers();
            services.AddSharedSecurity();
            await services.AddInfrastructure(configuration);

            services.AddWebAbstractions();
            services.AddAuthorizationAuthentification();
            services.AddMinimalApi();

            services.AddSwagger();

            return services;
        }
    }
}
