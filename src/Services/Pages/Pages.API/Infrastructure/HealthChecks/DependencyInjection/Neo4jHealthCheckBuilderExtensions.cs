
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Pages.API.Infrastructure.HealthChecks;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class Neo4jHealthCheckBuilderExtensions
    {
        const string NAME = "neo4j";

        /// <summary>
        /// Add a health check for neo4j graph databases.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="connectionString">The neo4j connection string to be used.</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'mysql' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddNeo4j(this IHealthChecksBuilder builder, string connectionString, string username = default, string password = default, string name = default, HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            return builder.Add(new HealthCheckRegistration(
                name ?? NAME,
                sp => new Neo4jHealthCheck(connectionString, username, password),
                failureStatus,
                tags));
        }
    }
}
