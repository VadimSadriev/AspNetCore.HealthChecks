using AspNetCore.HealthChecks.Data;
using AspNetCore.HealthChecks.HealthChecks.CustomHealthChecks;
using AspNetCore.HealthChecks.HealthChecks.CustomHealthChecks.Configuration;
using AspNetCore.HealthChecks.HealthChecks.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text.Json;

namespace AspNetCore.HealthChecks.HealthChecks
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CustomHealthCheckConfiguration>(configuration.GetSection("CustomHealthCheck"));

            services.AddHealthChecks()
                .AddDbContextCheck<DataContext>()
                .AddCheck<CustomHealthCheck>("CustomHealthCheck");

            return services;
        }

        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (httpContext, healthReport) =>
                {
                    httpContext.Response.ContentType = "application/json";

                    var response = new HealthCheckResponse
                    {
                        Status = healthReport.Status.ToString(),
                        Checks = healthReport.Entries.Select(x => new HealthCheck
                        {
                            Component = x.Key,
                            Status = x.Value.Status.ToString(),
                            Description = x.Value.Description
                        }),
                        Duration = healthReport.TotalDuration
                    };

                    var serializedResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    await httpContext.Response.WriteAsync(serializedResponse);
                }
            });

            return app;
        }
    }
}
