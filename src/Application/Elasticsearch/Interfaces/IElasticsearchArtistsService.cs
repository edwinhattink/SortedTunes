using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Models;

namespace SortedTunes.Application.Elasticsearch.Interfaces;

public interface IElasticsearchArtistsService
{
    Task IndexArtistAsync(ArtistDocument artist, CancellationToken cancellationToken);
    Task IndexArtistsAsync(IEnumerable<ArtistDocument> artists, CancellationToken cancellationToken);
    Task ResetIndexAsync(CancellationToken cancellationToken);
    Task<PaginatedList<ArtistDocument>> SearchArtistsAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task DeleteArtistAsync(int artistId, CancellationToken cancellationToken);
}
