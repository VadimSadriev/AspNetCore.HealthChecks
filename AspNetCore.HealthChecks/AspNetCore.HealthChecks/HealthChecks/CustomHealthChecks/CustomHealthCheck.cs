using AspNetCore.HealthChecks.HealthChecks.CustomHealthChecks.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCore.HealthChecks.HealthChecks.CustomHealthChecks
{
    public class CustomHealthCheck : IHealthCheck
    {
        private readonly IOptionsMonitor<CustomHealthCheckConfiguration> _options;

        public CustomHealthCheck(IOptionsMonitor<CustomHealthCheckConfiguration> options)
        {
            _options = options;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var connectionString = _options.CurrentValue.CustomConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                return HealthCheckResult.Unhealthy("Connection string required");
            }

            return HealthCheckResult.Healthy();
        }
    }
}
