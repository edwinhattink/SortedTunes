using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Ingest;
using SortedTunes.Application.Elasticsearch.Base;
using SortedTunes.Application.Elasticsearch.Exceptions;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Infrastructure.Services.Elasticsearch.Base;

namespace SortedTunes.Infrastructure.Services.Elasticsearch;

public class ElasticsearchLoggingService(ElasticsearchClient elasticsearchClient, TimeProvider dateTime)
    : BaseLoggingService(elasticsearchClient, dateTime), IElasticsearchLoggingService
{
    protected override string IndexPrefix => "cost-models-api";

    public override async Task AddLogging<T>(T request, long elapsedMilliseconds, CancellationToken cancellationToken = default)
    {
        await base.AddLogging(request, elapsedMilliseconds, cancellationToken);
    }

    protected override string? GetUserId()
    {
        return "Anonymous";
    }

    public async Task SetupLoggingSettings(CancellationToken cancellationToken)
    {
        const string pipelineName = "modify-cost-models-api-logging";

        var pipelineResponse = await _elasticsearchClient.Ingest.PutPipelineAsync(new PutPipelineRequest(pipelineName)
        {
            Description = "Pipeline to enrich data",
            Processors =
            [
                new EnrichProcessor
                {
                    PolicyName = "user-information-enrich-policy",
                    If = new() { Source = "ctx.userId != null && !ctx.userId.endsWith('@clients')" },
                    Field = new Field("userId"),
                    TargetField = new Field("userData"),
                },
                new ScriptProcessor
                {
                    If = new() { Source = "ctx.userId != null && ctx.userId.endsWith('@clients')" },
                    Source = @"
                        ctx.userData = new HashMap();
                        ctx.userData.authId = ctx.userId.replace('@clients', '');
                    "
                },
                new EnrichProcessor
                {
                    PolicyName = "application-information-enrich-policy",
                    If = new() { Source = "ctx.userData != null && ctx.userData.authId != null" },
                    Field = new Field("userData.authId"),
                    TargetField = new Field("userData"),
                },
                new RemoveProcessor() {
                    If = new() { Source = "ctx.userData != null && ctx.userData.authId != null" },
                    Field = new Field[] { new("userData.authId") }!
                }
            ]
        }, cancellationToken);

        // Check if the pipeline was created successfully
        if (!pipelineResponse.IsValidResponse)
        {
            throw new ElasticsearchException(pipelineResponse);
        }

        // Update the settings to attach the pipeline to the index
        var updateSettingsResponse = await _elasticsearchClient.Indices.PutSettingsAsync(new PutIndicesSettingsRequest(
            LogIndex(),
            new IndexSettings
            {
                DefaultPipeline = pipelineName
            }
        ), cancellationToken);

        // Check if the index settings were updated successfully
        if (!updateSettingsResponse.IsValidResponse)
        {
            throw new ElasticsearchException(updateSettingsResponse);
        }
    }
}
