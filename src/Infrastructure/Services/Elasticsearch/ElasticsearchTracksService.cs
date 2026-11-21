using Elastic.Clients.Elasticsearch;

using SortedTunes.Application.Common.Models;
using SortedTunes.Application.Elasticsearch.Interfaces;
using SortedTunes.Application.Elasticsearch.Models;
using SortedTunes.Infrastructure.Services.Elasticsearch.Base;

namespace SortedTunes.Infrastructure.Services.Elasticsearch;

public class ElasticsearchTracksService(ElasticsearchClient elasticsearchClient, TimeProvider timeProvider)
    : BaseElasticsearchService<TrackDocument>(elasticsearchClient, timeProvider), IElasticsearchTracksService
{
    protected override string IndexPrefix => "sorted-tunes-tracks";

    public Task IndexTrackAsync(TrackDocument track, CancellationToken cancellationToken)
        => IndexDocumentInternalAsync(track, track.Id, cancellationToken);

    public Task IndexTracksAsync(IEnumerable<TrackDocument> tracks, CancellationToken cancellationToken)
        => IndexDocumentsInternalAsync(tracks, t => t.Id, cancellationToken);

    public Task ResetIndexAsync(CancellationToken cancellationToken)
        => ResetIndexInternalAsync(cancellationToken);

    public Task<PaginatedList<TrackDocument>> SearchTracksAsync(int pageNumber, int pageSize, string? search = null, CancellationToken cancellationToken = default)
        => SearchInternalAsync(pageNumber, pageSize, search, cancellationToken);

    public Task DeleteTrackAsync(int trackId, CancellationToken cancellationToken)
        => DeleteDocumentInternalAsync(trackId, cancellationToken);

    protected override IEnumerable<Field> GetSearchFields()
    {
        yield return new Field("title");
        yield return new Field("title.search_as_you_type");
    }
}
