using SortedTunes.Application.Elasticsearch.MappingProperties;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Elasticsearch.Models;

public class AlbumDocument
{
    public static AlbumDocument Create(Album album) => new()
    {
        Id = album.Id,
        Title = album.Title,
        ReleaseYear = album.ReleaseYear,
        Image = album.Image,
        DiscCount = album.Discs?.Count ?? 0,
        Created = album.Created,
        CreatedBy = album.CreatedBy,
        LastModified = album.LastModified,
        LastModifiedBy = album.LastModifiedBy
    };

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
