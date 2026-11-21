using SortedTunes.Application.Elasticsearch.MappingProperties;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Elasticsearch.Models;

public class TrackDocument
{
    public static TrackDocument Create(Track track) => new()
    {
        Id = track.Id,
        Number = track.Number,
        Title = track.Title,
        FileName = track.FileName,
        DiscId = track.DiscId,
        Disc = track.Disc != null ? DiscDocument.Create(track.Disc) : null,
        GenreId = track.GenreId,
        Genre = track.Genre != null ? GenreDocument.Create(track.Genre) : null,
        Created = track.Created,
        CreatedBy = track.CreatedBy,
        LastModified = track.LastModified,
        LastModifiedBy = track.LastModifiedBy
    };

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
