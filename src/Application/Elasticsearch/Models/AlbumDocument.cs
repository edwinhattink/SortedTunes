using SortedTunes.Application.Elasticsearch.MappingProperties;

namespace SortedTunes.Application.Elasticsearch.Models;

public class AlbumDocument
{
    [KeywordField]
    public int Id { get; set; }

    [SearchField]
    [SearchAsYouTypeField]
    [KeywordField]
    public required string Title { get; set; }

    public int ReleaseYear { get; set; }

    [KeywordField]
    public string? Image { get; set; }

    public int DiscCount { get; set; }

    public DateTimeOffset Created { get; set; }

    [KeywordField]
    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    [KeywordField]
    public string? LastModifiedBy { get; set; }
}
