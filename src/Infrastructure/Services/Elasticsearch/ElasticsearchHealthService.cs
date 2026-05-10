using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SortedTunes.Infrastructure.Services.Elasticsearch;

public class ElasticsearchHealthService(ElasticsearchClient elasticsearchClient) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var response = await elasticsearchClient.PingAsync(cancellationToken);

        if (!response.IsValidResponse)
        {
            return HealthCheckResult.Unhealthy("Elasticsearch is unavailable");
        }

        return HealthCheckResult.Healthy("Elasticsearch is available.");
    }
}
