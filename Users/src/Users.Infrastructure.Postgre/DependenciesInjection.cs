using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Application.Repositories.Auth;
using Users.Infrastructure.Postgre.Repositories.Auth;

namespace Users.Infrastructure.Postgre
{
    public static class DependenciesInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IWriteAuthRepository, WriteAuthRepository>();
            services.AddScoped<IReadAuthRepository, ReadAuthRepository>();

            return services;
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<UsersDbContext>((options) =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });


            return services;
        }
    }
}
