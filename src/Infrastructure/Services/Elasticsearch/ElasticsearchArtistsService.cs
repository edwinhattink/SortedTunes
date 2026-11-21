using Elastic.Clients.Elasticsearch;

using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;
using SortedTunes.Infrastructure.Services.Elasticsearch.Base;

namespace SortedTunes.Infrastructure.Services.Elasticsearch;

public class ElasticsearchArtistsService(ElasticsearchClient elasticsearchClient, TimeProvider timeProvider)
    : BaseElasticsearchService<ArtistDocument>(elasticsearchClient, timeProvider), IElasticsearchArtistsService
{
    protected override string IndexPrefix => "sorted-tunes-artists";

    public Task IndexArtistAsync(ArtistDocument artist, CancellationToken cancellationToken)
        => IndexDocumentInternalAsync(artist, artist.Id, cancellationToken);

    public Task IndexArtistsAsync(IEnumerable<ArtistDocument> artists, CancellationToken cancellationToken)
        => IndexDocumentsInternalAsync(artists, a => a.Id, cancellationToken);

    public Task ResetIndexAsync(CancellationToken cancellationToken)
        => ResetIndexInternalAsync(cancellationToken);

    public Task<PaginatedList<ArtistDocument>> SearchArtistsAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        => SearchInternalAsync(pageNumber, pageSize, search, cancellationToken);

    public Task DeleteArtistAsync(int artistId, CancellationToken cancellationToken)
        => DeleteDocumentInternalAsync(artistId, cancellationToken);

    protected override IEnumerable<Field> GetSearchFields()
    {
        yield return new Field("name");
        yield return new Field("name.search_as_you_type");
    }
}
