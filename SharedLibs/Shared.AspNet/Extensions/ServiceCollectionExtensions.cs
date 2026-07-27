using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OpenTelemetry.Metrics;
using Shared.Objects.Classes.Options;

namespace Shared.AspNet.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelemetry(this IServiceCollection services)
        {
            services.AddOpenTelemetry().WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddPrometheusExporter();
            });

            return services;
        }

        public static IServiceCollection AddWebAbstractions(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            return services;    
        }

        public static IServiceCollection AddMinimalApi(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddValidation();

            return services;
        }

        public static IServiceCollection AddAuthorizationAuthentification(this IServiceCollection services)
        {
            services.AddJwtAuthentication();
            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var binDirectory = new DirectoryInfo(AppContext.BaseDirectory);
                var files = binDirectory.GetFiles("*.xml");

                foreach (var file in files)
                {
                    options.IncludeXmlComments(file.FullName);
                }

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var authOptions = new AuthOptions();

                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,

                    ValidateLifetime = true,

                    ValidateIssuer = true,
                    ValidIssuer = authOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudiences = authOptions.Audiences,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(authOptions.IssuerSigningKey),

                    RoleClaimType = "role"
                };
            });

            return services;
        }
    }
}
