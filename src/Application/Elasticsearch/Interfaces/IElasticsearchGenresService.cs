using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Models;

namespace SortedTunes.Application.Elasticsearch.Interfaces;

public interface IElasticsearchGenresService
{
    Task IndexGenreAsync(GenreDocument genre, CancellationToken cancellationToken);
    Task IndexGenresAsync(IEnumerable<GenreDocument> genres, CancellationToken cancellationToken);
    Task ResetIndexAsync(CancellationToken cancellationToken);
    Task<PaginatedList<GenreDocument>> SearchGenresAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task DeleteGenreAsync(int genreId, CancellationToken cancellationToken);
}
