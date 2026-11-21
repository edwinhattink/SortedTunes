using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Clients.Elasticsearch.QueryDsl;

using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Exceptions;
using SortedTunes.Application.Elasticsearch.Helpers;

namespace SortedTunes.Infrastructure.Services.Elasticsearch.Base;

public abstract class BaseElasticsearchService<TDocument>(ElasticsearchClient elasticsearchClient, TimeProvider timeProvider)
    where TDocument : class
{
    protected readonly ElasticsearchClient _elasticsearchClient = elasticsearchClient;
    protected readonly TimeProvider _timeProvider = timeProvider;

    protected abstract string IndexPrefix { get; }

    protected IndexName Index() => IndexPrefix;

    protected virtual async Task EnsureIndexExistsAsync(CancellationToken cancellationToken)
    {
        var existsResponse = await _elasticsearchClient.Indices.ExistsAsync(Index(), cancellationToken);

        if (existsResponse.Exists)
            return;

        var mappingProperties = MappingHelpers.GetMappingProperties(typeof(TDocument));

        var createResponse = await _elasticsearchClient.Indices.CreateAsync(new CreateIndexRequest(Index())
        {
            Settings = new IndexSettings
            {
                NumberOfShards = 1,
                NumberOfReplicas = 0
            },
            Mappings = new TypeMapping
            {
                Properties = new Properties(mappingProperties)
            }
        }, cancellationToken);

        if (!createResponse.IsValidResponse)
            throw new ElasticsearchException(createResponse);
    }

    protected virtual async Task ResetIndexInternalAsync(CancellationToken cancellationToken)
    {
        var existsResponse = await _elasticsearchClient.Indices.ExistsAsync(Index(), cancellationToken);

        if (existsResponse.Exists)
        {
            var deleteResponse = await _elasticsearchClient.Indices.DeleteAsync(Index(), cancellationToken);
            if (!deleteResponse.IsValidResponse)
                throw new ElasticsearchException(deleteResponse);
        }

        await EnsureIndexExistsAsync(cancellationToken);
    }

    protected virtual async Task IndexDocumentInternalAsync(TDocument document, int id, CancellationToken cancellationToken)
    {
        await EnsureIndexExistsAsync(cancellationToken);

        var response = await _elasticsearchClient.IndexAsync(document, Index(), id: id, cancellationToken: cancellationToken);

        if (!response.IsValidResponse)
            throw new ElasticsearchException(response);
    }

    protected virtual async Task IndexDocumentsInternalAsync(IEnumerable<TDocument> documents, Func<TDocument, int> idSelector, CancellationToken cancellationToken)
    {
        await EnsureIndexExistsAsync(cancellationToken);

        foreach (var doc in documents)
        {
            var response = await _elasticsearchClient.IndexAsync(doc, Index(), id: idSelector(doc), cancellationToken: cancellationToken);

            if (!response.IsValidResponse)
                throw new ElasticsearchException(response);
        }
    }

    protected virtual async Task DeleteDocumentInternalAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _elasticsearchClient.DeleteAsync<TDocument>(id, d => d.Index(Index()), cancellationToken);

        if (!response.IsValidResponse && response.ApiCallDetails?.HttpStatusCode != 404)
            throw new ElasticsearchException(response);
    }

    protected virtual async Task<PaginatedList<TDocument>> SearchInternalAsync(
        int pageNumber,
        int pageSize,
        string? search,
        CancellationToken cancellationToken = default)
    {
        await EnsureIndexExistsAsync(cancellationToken);

        var from = (pageNumber - 1) * pageSize;

        SearchResponse<TDocument> response;

        if (string.IsNullOrWhiteSpace(search))
        {
            response = await _elasticsearchClient.SearchAsync<TDocument>(s => s
                .Indices(Index())
                .From(from)
                .Size(pageSize)
                .Sort(sort => sort.Field(f => f.Field("id")))
                .Query(q => q.MatchAll(new MatchAllQuery())),
                cancellationToken);
        }
        else
        {
            var fields = GetSearchFields().ToArray();

            response = await _elasticsearchClient.SearchAsync<TDocument>(s => s
                .Indices(Index())
                .From(from)
                .Size(pageSize)
                .Query(q => q
                    .MultiMatch(mm => mm
                        .Query(search)
                        .Fields(fields)
                        .Type(TextQueryType.BestFields)
                        .Fuzziness(new Fuzziness("AUTO"))
                    )
                ),
                cancellationToken);
        }

        if (!response.IsValidResponse)
            throw new ElasticsearchException(response);

        var hits = response.Hits.Select(h => h.Source!).ToList();
        var total = response.HitsMetadata?.Total is { } t
            ? t.Match(th => (int)(th?.Value ?? 0), l => (int)l)
            : hits.Count;

        return new PaginatedList<TDocument>(hits, total, pageNumber, pageSize);
    }

    /// <summary>
    /// Override to specify which fields should be searched with multi_match.
    /// Defaults to "title" and "name".
    /// </summary>
    protected virtual IEnumerable<Field> GetSearchFields()
    {
        yield return new Field("title");
        yield return new Field("name");
    }
}
