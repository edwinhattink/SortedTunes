using SortedTunes.Application.Elasticsearch.MappingProperties;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Elasticsearch.Models;

public class DiscDocument
{
    public static DiscDocument Create(Disc disc) => new()
    {
        Id = disc.Id,
        Number = disc.Number,
        Title = disc.Title,
        AlbumId = disc.AlbumId,
        Album = disc.Album != null ? AlbumDocument.Create(disc.Album) : null,
        Tracks = disc.Tracks?.Select(TrackDocument.Create).ToList() ?? [],
        Created = disc.Created,
        CreatedBy = disc.CreatedBy,
        LastModified = disc.LastModified,
        LastModifiedBy = disc.LastModifiedBy
    };

    [KeywordField]
    public int Id { get; set; }

    public int Number { get; set; }

    [SearchField]
    [KeywordField]
    public string? Title { get; set; }

    [KeywordField]
    public int AlbumId { get; set; }

    [NestedField]
    public AlbumDocument? Album { get; set; }

    [NestedField]
    public List<TrackDocument> Tracks { get; set; } = [];

    public DateTimeOffset Created { get; set; }

    [KeywordField]
    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    [KeywordField]
    public string? LastModifiedBy { get; set; }
}
