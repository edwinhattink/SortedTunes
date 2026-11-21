using SortedTunes.Application.Elasticsearch.MappingProperties;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Elasticsearch.Models;

public class GenreDocument
{
    public static GenreDocument Create(Genre genre) => new()
    {
        Id = genre.Id,
        Name = genre.Name,
        ParentGenreId = genre.ParentGenreId,
        ParentGenre = genre.ParentGenre != null ? Create(genre.ParentGenre) : null,
        Tracks = genre.Tracks?.Select(TrackDocument.Create).ToList() ?? [],
        Created = genre.Created,
        CreatedBy = genre.CreatedBy,
        LastModified = genre.LastModified,
        LastModifiedBy = genre.LastModifiedBy
    };

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
