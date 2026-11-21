using Elastic.Clients.Elasticsearch;

using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;
using SortedTunes.Infrastructure.Services.Elasticsearch.Base;

namespace SortedTunes.Infrastructure.Services.Elasticsearch;

public class ElasticsearchDiscsService(ElasticsearchClient elasticsearchClient, TimeProvider timeProvider)
    : BaseElasticsearchService<DiscDocument>(elasticsearchClient, timeProvider), IElasticsearchDiscsService
{
    protected override string IndexPrefix => "sorted-tunes-discs";

    public Task IndexDiscAsync(DiscDocument disc, CancellationToken cancellationToken)
        => IndexDocumentInternalAsync(disc, disc.Id, cancellationToken);

    public Task IndexDiscsAsync(IEnumerable<DiscDocument> discs, CancellationToken cancellationToken)
        => IndexDocumentsInternalAsync(discs, d => d.Id, cancellationToken);

    public Task ResetIndexAsync(CancellationToken cancellationToken)
        => ResetIndexInternalAsync(cancellationToken);

    public Task<PaginatedList<DiscDocument>> SearchDiscsAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        => SearchInternalAsync(pageNumber, pageSize, search, cancellationToken);

    public Task DeleteDiscAsync(int discId, CancellationToken cancellationToken)
        => DeleteDocumentInternalAsync(discId, cancellationToken);

    protected override IEnumerable<Field> GetSearchFields()
    {
        yield return new Field("title");
    }
}
