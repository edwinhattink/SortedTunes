using Elastic.Clients.Elasticsearch;

using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;
using SortedTunes.Infrastructure.Services.Elasticsearch.Base;

namespace SortedTunes.Infrastructure.Services.Elasticsearch;

public class ElasticsearchAlbumsService(ElasticsearchClient elasticsearchClient, TimeProvider timeProvider)
    : BaseElasticsearchService<AlbumDocument>(elasticsearchClient, timeProvider), IElasticsearchAlbumsService
{
    protected override string IndexPrefix => "sorted-tunes-albums";

    public Task IndexAlbumAsync(AlbumDocument album, CancellationToken cancellationToken)
        => IndexDocumentInternalAsync(album, album.Id, cancellationToken);

    public Task IndexAlbumsAsync(IEnumerable<AlbumDocument> albums, CancellationToken cancellationToken)
        => IndexDocumentsInternalAsync(albums, a => a.Id, cancellationToken);

    public Task ResetIndexAsync(CancellationToken cancellationToken)
        => ResetIndexInternalAsync(cancellationToken);

    public Task<PaginatedList<AlbumDocument>> SearchAlbumsAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        => SearchInternalAsync(pageNumber, pageSize, search, cancellationToken);

    public Task DeleteAlbumAsync(int albumId, CancellationToken cancellationToken)
        => DeleteDocumentInternalAsync(albumId, cancellationToken);

    protected override IEnumerable<Field> GetSearchFields()
    {
        yield return new Field("title");
        yield return new Field("title.search_as_you_type");
    }
}
