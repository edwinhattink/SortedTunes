using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Models;

namespace SortedTunes.Application.Elasticsearch.Interfaces;

public interface IElasticsearchAlbumsService
{
    Task IndexAlbumAsync(AlbumDocument album, CancellationToken cancellationToken);
    Task IndexAlbumsAsync(IEnumerable<AlbumDocument> albums, CancellationToken cancellationToken);
    Task ResetIndexAsync(CancellationToken cancellationToken);
    Task<PaginatedList<AlbumDocument>> SearchAlbumsAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task DeleteAlbumAsync(int albumId, CancellationToken cancellationToken);
}
