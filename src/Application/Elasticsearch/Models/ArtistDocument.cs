using SortedTunes.Application.Elasticsearch.MappingProperties;
using SortedTunes.Domain.Entities;

namespace SortedTunes.Application.Elasticsearch.Models;

public class ArtistDocument
{
    public static ArtistDocument Create(Artist artist) => new()
    {
        Id = artist.Id,
        Name = artist.Name,
        Created = artist.Created,
        CreatedBy = artist.CreatedBy,
        LastModified = artist.LastModified,
        LastModifiedBy = artist.LastModifiedBy
    };

    [KeywordField]
    public int Id { get; set; }

    [SearchField]
    [SearchAsYouTypeField]
    [KeywordField]
    public required string Name { get; set; }

    public DateTimeOffset Created { get; set; }

    [KeywordField]
    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModified { get; set; }

    [KeywordField]
    public string? LastModifiedBy { get; set; }
}
