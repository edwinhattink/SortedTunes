using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Models;

namespace SortedTunes.Application.Elasticsearch.Interfaces;

public interface IElasticsearchDiscsService
{
    Task IndexDiscAsync(DiscDocument disc, CancellationToken cancellationToken);
    Task IndexDiscsAsync(IEnumerable<DiscDocument> discs, CancellationToken cancellationToken);
    Task ResetIndexAsync(CancellationToken cancellationToken);
    Task<PaginatedList<DiscDocument>> SearchDiscsAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task DeleteDiscAsync(int discId, CancellationToken cancellationToken);
}
