using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using SortedTunes.Application.Elasticsearch.Exceptions;
using SortedTunes.Application.Elasticsearch.Helpers;
using SortedTunes.Infrastructure.Services.Elasticsearch.Models.Logs;

namespace SortedTunes.Infrastructure.Services.Elasticsearch.Base;

public abstract class BaseLoggingService(ElasticsearchClient elasticsearchClient, TimeProvider timeProvider)
{
    protected readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;
    protected readonly TimeProvider _timeProvider = timeProvider;

    protected abstract string IndexPrefix { get; }

    protected abstract string? GetUserId();

    protected IndexName SearchLogIndex() => $"{IndexPrefix}-search-logging";
    protected IndexName LogIndex() => $"{IndexPrefix}-logging";
    protected IndexName ExceptionLogIndex() => $"{IndexPrefix}-exception-logging";

    public async virtual Task AddSearchLogging<T>(T request, int? resultsCount, CancellationToken cancellationToken = default)
    {
        await _elasticsearchClient.IndexAsync(new SearchLog<T>
        {
            UserId = GetUserId(),
            DateTime = _timeProvider.GetUtcNow().DateTime,
            RequestName = typeof(T).Name,
            Request = request,
            ResultsCount = resultsCount
        }, SearchLogIndex(), id: null, cancellationToken: cancellationToken);
    }

    public async virtual Task AddLogging<T>(T request, long elapsedMilliseconds, CancellationToken cancellationToken = default)
    {
        await _elasticsearchClient.IndexAsync(new Log<T>
        {
            UserId = GetUserId(),
            DateTime = _timeProvider.GetUtcNow().DateTime,
            RequestName = typeof(T).Name,
            Request = request,
            ElapsedMilliseconds = elapsedMilliseconds,
        }, LogIndex(), id: null, cancellationToken: cancellationToken);
    }

    public async virtual Task ExceptionLogging<T>(T request, Exception exception, CancellationToken cancellationToken = default)
    {
        await _elasticsearchClient.IndexAsync(new ExceptionLog<T>
        {
            UserId = GetUserId(),
            DateTime = _timeProvider.GetUtcNow().DateTime,
            RequestName = typeof(T).Name,
            ExceptionType = exception.GetType().ToString(),
            Request = request,
            Message = exception.Message,
            Stacktrace = exception.StackTrace
        }, ExceptionLogIndex(), id: null, cancellationToken: cancellationToken);
    }

    private string DateTimeString()
    {
        return _timeProvider.GetUtcNow().ToString("yyyyMMdd-HHmm");
    }

    public async virtual Task ResetLoggingIndices(CancellationToken cancellationToken)
    {
        await ResetLogIndex<Log<object>>(LogIndex(), cancellationToken);
        await ResetLogIndex<ExceptionLog<object>>(ExceptionLogIndex(), cancellationToken);
        await ResetLogIndex<SearchLog<object>>(SearchLogIndex(), cancellationToken);
    }

    protected async Task ResetLogIndex<T>(IndexName name, CancellationToken cancellationToken)
    {
        var indexName = $"{name}-{DateTimeString()}";
        var mappingProperties = MappingHelpers.GetMappingProperties(typeof(T));
        var createIndexResponse = await _elasticsearchClient.Indices.CreateAsync(new CreateIndexRequest(indexName)
        {
            Settings = new IndexSettings() { },
            Mappings = new TypeMapping
            {
                Properties = new Properties(mappingProperties)
            }
        }, cancellationToken);
        if (!createIndexResponse.IsValidResponse)
        {
            throw new ElasticsearchException(createIndexResponse);
        }

        var updateAliasResponse = await _elasticsearchClient.Indices.UpdateAliasesAsync(new UpdateAliasesRequest()
        {
            Actions = [
                new AddAction()
                {
                    Index = indexName,
                    Alias = $"{name}",
                    IsWriteIndex = true,
                }
            ]
        }, cancellationToken);
        if (!updateAliasResponse.IsValidResponse)
        {
            throw new ElasticsearchException(updateAliasResponse);
        }
    }
}
