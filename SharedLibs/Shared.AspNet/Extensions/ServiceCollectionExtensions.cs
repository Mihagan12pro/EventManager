using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Shared.AspNet.Options;
using Shared.Objects.Classes;
using System.Text;
using System.Text.Json;

namespace Shared.AspNet.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTelemetry(
            this IServiceCollection services,
            IConfiguration configuration,
            string serviceName)
        {

            services.AddOpenTelemetry().WithMetrics(metrics =>
                {
                    metrics
                        .AddAspNetCoreInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddPrometheusExporter();
                })
                .WithTracing(tracerProviderBuilder =>
                {
                    var batchSection = configuration.GetRequiredSection(
                        "JaegerOptions:BatchExportOptions"
                    );

                    var otlpSection = configuration.GetRequiredSection(
                        "JaegerOptions:OtlpExporterOptions"
                    );

                    var JaegerOptions = new JaegerOptions
                    {
                        BatchExportOptions = new BatchExportOptions
                        {
                            ExporterTimeoutMilliseconds = int.Parse(
                                 batchSection["ExporterTimeoutMilliseconds"]),

                            ScheduledDelayMilliseconds = int.Parse(
                                 batchSection["ScheduledDelayMilliseconds"]),
                        },

                        OtlpExportOptions = new Options.OtlpExporterOptions
                        {
                            EndPoint = otlpSection["EndPoint"],

                            Protocol = Enum.Parse<OtlpExportProtocol>(
                                otlpSection["Protocol"])
                        }
                    };


                    tracerProviderBuilder.SetResourceBuilder(
                                            ResourceBuilder.CreateDefault()
                                            .AddService(serviceName))
                               .AddAspNetCoreInstrumentation(options =>
                               {
                                     options.Filter = httpContext =>
                                     {
                                         var path = httpContext.Request.Path;

                                         return !path.StartsWithSegments("/health") &&
                                                !path.StartsWithSegments("/metrics");
                                     };
                               })
                                .AddOtlpExporter(options =>
                                {
                                    options.Endpoint = JaegerOptions.OtlpExportOptions.EndPointUri;
                                    options.Protocol = JaegerOptions.OtlpExportOptions.Protocol;
                                    options.BatchExportProcessorOptions.ScheduledDelayMilliseconds = JaegerOptions.BatchExportOptions.ScheduledDelayMilliseconds;
                                    options.BatchExportProcessorOptions.ExporterTimeoutMilliseconds = JaegerOptions.BatchExportOptions.ExporterTimeoutMilliseconds;
                                })
                               .AddHttpClientInstrumentation();
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

        public static IServiceCollection AddAuthorizationAuthentification(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddJwtAuthentication(configuration);
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

        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtToken = new JwtToken();


                var jwtSection = configuration.GetRequiredSection("JwtOptions");

                jwtToken.Issuer = jwtSection.GetRequiredSection("Issuer").Value;
                jwtToken.ExpiredMinutes = jwtSection.GetRequiredSection("ExpiredMinutes").Value;
                jwtToken.IssuerSigningKey = Encoding.UTF8.GetBytes(jwtSection.GetRequiredSection("SecretKey").Value);
                jwtToken.Audiences = new List<string>();

                jwtToken.Audiences = new List<string>();

                foreach (var section in jwtSection.GetRequiredSection("Audiences").GetChildren())
                    ((List<string>)jwtToken.Audiences).Add(section.Value);

                options.MapInboundClaims = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero,

                    ValidateLifetime = true,

                    ValidateIssuer = true,
                    ValidIssuer = jwtToken.Issuer,

                    ValidateAudience = true,
                    ValidAudiences = jwtToken.Audiences,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtToken.IssuerSigningKey),

                    RoleClaimType = "role"
                };
            });

            return services;
        }
    }
}
