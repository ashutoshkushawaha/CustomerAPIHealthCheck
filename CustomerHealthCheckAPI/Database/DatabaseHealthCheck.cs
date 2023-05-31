using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;

namespace CustomerHealthCheckAPI.Database
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public DatabaseHealthCheck(string connectionString)
        {
            _connectionString = connectionString ?? "Server=.;Database=CustomerHealthCheckAPIContextNew;TrustServerCertificate=True;Integrated Security=true;MultipleActiveResultSets=true;Timeout=30;";
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT 1";
                    var result = await command.ExecuteScalarAsync(cancellationToken);

                    if (result != null)
                    {
                        return HealthCheckResult.Healthy();
                    }
                    else
                    {
                        return HealthCheckResult.Unhealthy("Database is not responding correctly.");
                    }
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"An exception occurred while checking the database health: {ex.Message}");
            }
        }
    }
}