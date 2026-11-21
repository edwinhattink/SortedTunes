using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Models;

namespace SortedTunes.Application.Elasticsearch.Interfaces;

public interface IElasticsearchTracksService
{
    Task IndexTrackAsync(TrackDocument track, CancellationToken cancellationToken);
    Task IndexTracksAsync(IEnumerable<TrackDocument> tracks, CancellationToken cancellationToken);
    Task ResetIndexAsync(CancellationToken cancellationToken);
    Task<PaginatedList<TrackDocument>> SearchTracksAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default);
    Task DeleteTrackAsync(int trackId, CancellationToken cancellationToken);
}
