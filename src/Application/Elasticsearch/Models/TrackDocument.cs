using SortedTunes.Application.Elasticsearch.MappingProperties;

namespace SortedTunes.Application.Elasticsearch.Models;

public class TrackDocument
{
    [KeywordField]
    public int Id { get; set; }

    public int? Number { get; set; }

    [SearchField]
    [SearchAsYouTypeField]
    [KeywordField]
    public required string Title { get; set; }

    [KeywordField]
    public string? FileName { get; set; }

    [KeywordField]
    public int DiscId { get; set; }

    [NestedField]
    public DiscDocument? Disc { get; set; }

    [KeywordField]
    public int GenreId { get; set; }

    [NestedField]
    public GenreDocument? Genre { get; set; }

    public DateTimeOffset Created { get; set; }

    [KeywordField]
    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    [KeywordField]
    public string? LastModifiedBy { get; set; }
}
