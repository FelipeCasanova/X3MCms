using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Neo4jClient;

namespace Pages.API.Infrastructure.HealthChecks
{
    public class Neo4jHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly string _username;
        private readonly string _password;

        public Neo4jHealthCheck(string connectionString, string username = default, string password = default)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _username = username ?? throw new ArgumentNullException(nameof(username));
            _password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var connection = new GraphClient(new Uri(_connectionString), _username, _password))
                {
                    await connection.ConnectAsync();
                    if (!connection.IsConnected)
                    {
                        return new HealthCheckResult(context.Registration.FailureStatus, description: $"The {nameof(Neo4jHealthCheck)} check fail.");
                    }
                }
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }
        }
    }
}
