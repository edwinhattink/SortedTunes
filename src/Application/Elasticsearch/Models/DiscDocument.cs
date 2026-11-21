using SortedTunes.Application.Elasticsearch.MappingProperties;

namespace SortedTunes.Application.Elasticsearch.Models;

public class DiscDocument
{
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
