using CustomerHealthCheckAPI.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CustomerHealthCheckAPI.Health
{
    public class GitHubHealthChecker : IHealthCheck
    {
        private readonly IGitHubService _gitHubService;
        public GitHubHealthChecker(IGitHubService gitHubService) {
            _gitHubService=gitHubService;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
             var res = await _gitHubService.IsValidGitHubUser("ashutoshkushawaha");
                if (res)
                {
                    return HealthCheckResult.Healthy();

                }
                return HealthCheckResult.Unhealthy();
            }
            catch (Exception ex)
            {

                return HealthCheckResult.Unhealthy(ex.Message);
            }

        }
    }
}
