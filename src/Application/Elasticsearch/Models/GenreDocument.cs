using SortedTunes.Application.Elasticsearch.MappingProperties;

namespace SortedTunes.Application.Elasticsearch.Models;

public class GenreDocument
{
    [KeywordField]
    public int Id { get; set; }

    [SearchField]
    [SearchAsYouTypeField]
    [KeywordField]
    public required string Name { get; set; }

    [KeywordField]
    public int? ParentGenreId { get; set; }

    [NestedField]
    public GenreDocument? ParentGenre { get; set; }

    [NestedField]
    public List<TrackDocument> Tracks { get; set; } = [];

    public DateTimeOffset Created { get; set; }

    [KeywordField]
    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    [KeywordField]
    public string? LastModifiedBy { get; set; }
}
